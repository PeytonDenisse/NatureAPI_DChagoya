namespace NatureAPI.Models.Entities;

public class PlaceAmenity
{
    // Fk 
    public int PlaceId { get; set; }
    public int AmenityId { get; set; }

    // navegación
    public Place Place { get; set; }
    public Amenity Amenity { get; set; }
}