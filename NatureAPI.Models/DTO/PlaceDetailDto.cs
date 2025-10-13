namespace NatureAPI.Models;

public class PlaceDetailDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Category { get; init; } = null!;
    public string? Description { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public int? ElevationMeters { get; init; }
    public bool Accessible { get; init; }
    public double? EntryFee { get; init; }
    public string? OpeningHours { get; init; }

    public IEnumerable<string> Amenities { get; init; } = Array.Empty<string>();
    public IEnumerable<PhotoDto> Photos { get; init; } = Array.Empty<PhotoDto>();
    public IEnumerable<TrailDto> Trails { get; init; } = Array.Empty<TrailDto>();
}