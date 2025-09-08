namespace NatureAPI.Models.Entities;

public class PlaceAmenity
{
    // FKs
    public int PlaceId { get; set; }
    public int AmenityId { get; set; }

    // navegación
    public Place Place { get; set; } = null!;
    public Amenity Amenity { get; set; } = null!;
}