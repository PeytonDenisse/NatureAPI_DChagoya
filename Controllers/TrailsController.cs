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
public class TrailsController : ControllerBase
{
    private readonly NatureDbContext _db;
    private readonly IConfiguration _config;
    public TrailsController(NatureDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

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
                t.IsLoop,
                t.Path))
            .ToListAsync();

        return Ok(data);
    }
    
    // POST /api/trails/bulk
    [HttpPost("bulk")]
    public async Task<ActionResult> CreateBulk([FromBody] List<TrailCreateDto> trails)
    {
        if (trails is null || trails.Count == 0)
            return BadRequest("No se recibieron senderos.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        // Validar que todos los PlaceId existan
        var placeIds = trails.Select(t => t.PlaceId).Distinct().ToList();

        var existingPlaceIds = await _db.Places
            .Where(p => placeIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync();

        var missing = placeIds.Except(existingPlaceIds).ToList();
        if (missing.Any())
        {
            return BadRequest($"Algunos PlaceId no existen en la BD: {string.Join(", ", missing)}");
        }

        var newTrails = trails
            .Select(dto => new Trail
            {
                PlaceId = dto.PlaceId,
                Name = dto.Name,
                DistanceKm = dto.DistanceKm,
                EstimatedTimeMinutes = dto.EstimatedTimeMinutes,
                Difficulty = dto.Difficulty,
                Path = dto.Path,
                IsLoop = dto.IsLoop
            })
            .ToList();

        _db.Trails.AddRange(newTrails);
        await _db.SaveChangesAsync();

        var response = newTrails.Select(t => new
        {
            t.Id,
            t.Name,
            t.PlaceId,
            t.DistanceKm,
            t.EstimatedTimeMinutes,
            t.Difficulty,
            t.IsLoop
        });

        return Ok(new
        {
            message = "Senderos creados correctamente.",
            count = newTrails.Count,
            trails = response
        });
    }
    
    // GET /api/trails/ai-analyze
    [HttpGet("ai-analyze")]
    public async Task<ActionResult> AnalyzeTrails()
    {
        var apiKey = _config["OpenAIKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            return StatusCode(500, "OpenAIKey no está configurada.");

        var client = new ChatClient("gpt-5-mini", apiKey);

        var trails = await _db.Trails
            .Include(t => t.Place)
            .AsNoTracking()
            .ToListAsync();

        var summary = trails.Select(t => new
        {
            t.Id,
            t.Name,
            t.DistanceKm,
            t.EstimatedTimeMinutes,
            t.Difficulty,
            t.IsLoop,
            Place = new
            {
                t.Place.Id,
                t.Place.Name,
                t.Place.Category
            }
        });

        var jsonData = JsonSerializer.Serialize(summary);
        var prompt = Prompts.GenerateTrailsPrompt(jsonData);

        var result = await client.CompleteChatAsync([
            new UserChatMessage(prompt)
        ]);

        var response = result.Value.Content[0].Text;

        return Content(response, "application/json");
    }
}

    
    
