
namespace NatureAPI.Models.Entities;

public class Trail
{
    public int Id { get; set; }
    public int PlaceId { get; set; } // Fk
    public string Name { get; set; }
    public double DistanceKm { get; set; }
    public int EstimatedTimeMinutes { get; set; }
    public string Difficulty { get; set; } // Fácil, Moderado, Difícil
    public string Path { get; set; } // Podría coordenadas 
    public bool IsLoop { get; set; }

    // navegación 
    public Place Place { get; set; }
}