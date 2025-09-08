using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NatureAPI.Data;
using NatureAPI.Models;
using NatureAPI.Models.Entities;

namespace NatureAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlacesController : ControllerBase
{
    private readonly NatureDbContext _db;
    public PlacesController(NatureDbContext db) => _db = db;

   

    // GET /api/places?category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Place>>> GetAll(
        [FromQuery] string? category,
        [FromQuery] string? difficulty)
    {
        var q = _db.Places.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            q = q.Where(p => p.Category == category);

        if (!string.IsNullOrWhiteSpace(difficulty))
            q = q.Where(p => p.Trails.Any(t => t.Difficulty == difficulty));

        var list = await q
            .Include(p => p.Trails)
            .Include(p => p.Photos)
            .Include(p => p.PlaceAmenities).ThenInclude(pa => pa.Amenity)
            .ToListAsync();

        return Ok(list);
    }

    // GET /api/places/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Place>> GetById(int id)
    {
        var place = await _db.Places
            .Include(p => p.Trails)
            .Include(p => p.Photos)
            .Include(p => p.Reviews)
            .Include(p => p.PlaceAmenities).ThenInclude(pa => pa.Amenity)
            .FirstOrDefaultAsync(p => p.Id == id);

        return place is null ? NotFound() : Ok(place);
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
