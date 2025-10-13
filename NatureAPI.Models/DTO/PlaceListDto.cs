namespace NatureAPI.Models;

public class PlaceListDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Category { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    public PlaceListDto(int id, string name, string category, double latitude, double longitude)
    {
        Id = id;
        Name = name;
        Category = category;
        Latitude = latitude;
        Longitude = longitude;
    }
}