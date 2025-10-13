using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NatureAPI.Data;
using NatureAPI.Models;             // <- PlaceListDto
using NatureAPI.Models.Entities;    // <- Entidades (Place, Trail, etc.)

namespace NatureAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlacesController : ControllerBase
{
    private readonly NatureDbContext _db;
    public PlacesController(NatureDbContext db) => _db = db;

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
}
