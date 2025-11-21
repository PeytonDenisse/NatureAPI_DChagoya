using System.ComponentModel.DataAnnotations;

namespace NatureAPI.Models;

public class TrailCreateDto
{
    // FK al lugar al que pertenece el sendero
    [Required]
    public int PlaceId { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = null!;

    // Distancia en kilómetros
    [Range(0.1, double.MaxValue, ErrorMessage = "La distancia debe ser mayor a 0.")]
    public double DistanceKm { get; set; }

    // Tiempo estimado en minutos
    [Range(1, int.MaxValue, ErrorMessage = "El tiempo debe ser mayor a 0.")]
    public int EstimatedTimeMinutes { get; set; }

    // Fácil / Media / Alta (o lo que uses)
    [Required, StringLength(20)]
    public string Difficulty { get; set; } = null!;

    // GeoJSON, lista de puntos, etc. Opcional
    public string? Path { get; set; }

    // Indica si es circuito cerrado
    public bool IsLoop { get; set; }
}