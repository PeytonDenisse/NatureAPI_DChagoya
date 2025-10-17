using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NatureAPI.Data;
using NatureAPI.Models;   

namespace NatureAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrailsController : ControllerBase
{
    private readonly NatureDbContext _db;
    public TrailsController(NatureDbContext db) => _db = db;

    // GET /api/trails
    // Respuesta ligera con lo que pide el examen
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrailDto>>> GetAll()
    {
        var data = await _db.Trails
            .AsNoTracking()
            .Select(t => new TrailDto(
                t.Id,
                t.Name,
                t.DistanceKm,
                t.EstimatedTimeMinutes,
                t.Difficulty,
                t.IsLoop))
            .ToListAsync();

        return Ok(data);
    }
}