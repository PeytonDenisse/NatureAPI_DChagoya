using System.ComponentModel.DataAnnotations;

namespace NatureAPI.Models.Entities;

public class Trail
{
    public int Id { get; set; }
    public int PlaceId { get; set; }  // FK

    [Required, StringLength(120)]
    public string Name { get; set; } = null!;

    public double DistanceKm { get; set; }
    public int EstimatedTimeMinutes { get; set; }

    [Required, StringLength(20)]  // Fácil/Moderado/Dificil
    public string Difficulty { get; set; } = null!;

    public string? Path { get; set; }   
    public bool IsLoop { get; set; }

    // navegación
    public Place Place { get; set; } = null!;
}