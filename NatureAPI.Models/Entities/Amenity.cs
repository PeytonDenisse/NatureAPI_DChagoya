namespace NatureAPI.Models.Entities;

public class Amenity
{
    public int Id { get; set; }
    public string Name { get; set; }

    // navegación 
    public ICollection<PlaceAmenity> PlaceAmenities { get; set; }
}