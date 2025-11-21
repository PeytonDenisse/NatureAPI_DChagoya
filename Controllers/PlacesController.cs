using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NatureAPI.Data;
using NatureAPI.Models;            
using NatureAPI.Models.Entities; 
using OpenAI.Chat;          // 👈 NUEVO
using System.Text.Json;     // 👈 NUEVO


namespace NatureAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlacesController : ControllerBase
{
    private readonly NatureDbContext _db;
    private readonly IConfiguration _config; 
    
    public PlacesController(NatureDbContext db, IConfiguration config) // 👈 NUEVO
    {
        _db = db;
        _config = config;
    }

    // GET /api/places?category=parque&difficulty=Fácil
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlaceListDto>>> GetAll(
        [FromQuery] string? category,
        [FromQuery] string? difficulty)
    {
        var q = _db.Places.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            q = q.Where(p => p.Category == category);

        if (!string.IsNullOrWhiteSpace(difficulty))
            q = q.Where(p => p.Trails.Any(t => t.Difficulty == difficulty));

        var list = await q
            .Select(p => new PlaceListDto(
                p.Id,
                p.Name,
                p.Category,
                p.Latitude,
                p.Longitude))
            .ToListAsync();

        return Ok(list);
    }

    // GET /api/places/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlaceDetailDto>> GetById(int id)
    {
        var p = await _db.Places
            .Include(x => x.PlaceAmenities).ThenInclude(pa => pa.Amenity)
            .Include(x => x.Photos)
            .Include(x => x.Trails)
            .Include(x => x.Reviews)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p is null) return NotFound();

        var dto = new PlaceDetailDto
        {
            Id = p.Id,
            Name = p.Name,
            Category = p.Category,
            Description = p.Description,
            Latitude = p.Latitude,
            Longitude = p.Longitude,
            ElevationMeters = p.ElevationMeters,
            Accessible = p.Accessible,
            EntryFee = p.EntryFee,
            OpeningHours = p.OpeningHours,
            AverageRating = p.Reviews.Any()
                ? Math.Round(p.Reviews.Average(r => r.Rating), 1)  // ej. 4.3
                : (double?)null,
            ReviewCount = p.Reviews.Count,
            Amenities = p.PlaceAmenities.Select(a => a.Amenity.Name).ToList(),
            Photos = p.Photos.Select(ph => new PhotoDto(ph.Id, ph.Url, ph.Description)).ToList(),
            Trails = p.Trails.Select(t => new TrailDto(t.Id, t.Name, t.DistanceKm, t.EstimatedTimeMinutes, t.Difficulty, t.IsLoop)).ToList()
        };

        return Ok(dto);
    }



    // POST /api/places
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] PlaceCreateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var place = new Place
        {
            Name = dto.Name,
            Description = dto.Description,
            Category = dto.Category,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            ElevationMeters = dto.ElevationMeters,
            Accessible = dto.Accessible,
            EntryFee = dto.EntryFee,
            OpeningHours = dto.OpeningHours,
            CreatedAt = DateTime.UtcNow
        };

        _db.Places.Add(place);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = place.Id }, place);
    }
    
    
    // POST /api/places/bulk
    [HttpPost("bulk")]
    public async Task<ActionResult> CreateBulk([FromBody] List<PlaceCreateDto> places)
    {
        if (places is null || places.Count == 0)
            return BadRequest("No se recibieron lugares.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var newPlaces = new List<Place>();

            foreach (var dto in places)
            {
                var place = new Place
                {
                    Name = dto.Name,
                    Description = dto.Description ?? string.Empty,
                    Category = dto.Category,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    ElevationMeters = dto.ElevationMeters,
                    Accessible = dto.Accessible,
                    EntryFee = dto.EntryFee,
                    OpeningHours = dto.OpeningHours ?? string.Empty,
                    CreatedAt = DateTime.UtcNow
                };

                // Agregar fotos (URL + descripción)
                if (dto.Photos != null && dto.Photos.Count > 0)
                {
                    foreach (var ph in dto.Photos)
                    {
                        place.Photos.Add(new Photo
                        {
                            Url = ph.Url,
                            Description = ph.Description
                        });
                    }
                }

                newPlaces.Add(place);
            }

            _db.Places.AddRange(newPlaces);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            var response = newPlaces.Select(p => new
            {
                p.Id,
                p.Name,
                p.Category,
                p.Latitude,
                p.Longitude,
                photoCount = p.Photos.Count
            });

            return Ok(new
            {
                message = "Lugares creados correctamente.",
                count = newPlaces.Count,
                places = response
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("ai-analyzi")]
    public async Task<ActionResult> AnalyzePlaces()
    {
        var apiKey = _config["OpenAIKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            return StatusCode(500, "OpenAIKey no está configurada.");

        var client = new ChatClient("gpt-5-mini", apiKey);

        var places = await _db.Places
            .Include(p => p.Reviews)
            .Include(p => p.Trails)
            .Include(p => p.Photos)
            .AsNoTracking()
            .ToListAsync();

        var summary = places.Select(p => new
        {
            p.Id,
            p.Name,
            p.Category,
            p.Latitude,
            p.Longitude,
            p.Accessible,
            p.EntryFee,
            AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
            ReviewCount = p.Reviews.Count,
            TrailCount = p.Trails.Count,
            PhotosCount = p.Photos.Count
        });

        var jsonData = JsonSerializer.Serialize(summary);
        var prompt = Prompts.GeneratePlacesPrompt(jsonData);

        var result = await client.CompleteChatAsync([
            new UserChatMessage(prompt)
        ]);

        var response = result.Value.Content[0].Text;

        return Content(response, "application/json");
    }
    


}
