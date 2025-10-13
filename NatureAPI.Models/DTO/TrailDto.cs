namespace NatureAPI.Models;

public class TrailDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public double DistanceKm { get; init; }
    public int EstimatedTimeMinutes { get; init; }
    public string Difficulty { get; init; } = null!;
    public bool IsLoop { get; init; }

    public TrailDto(int id, string name, double distanceKm, int estimatedTimeMinutes, string difficulty, bool isLoop)
    {
        Id = id;
        Name = name;
        DistanceKm = distanceKm;
        EstimatedTimeMinutes = estimatedTimeMinutes;
        Difficulty = difficulty;
        IsLoop = isLoop;
    }
}