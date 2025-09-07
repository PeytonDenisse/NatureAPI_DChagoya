

namespace NatureAPI.Models.Entities;

public class Place
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; } // Parque, Cascada, Mirador
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int ElevationMeters { get; set; }
    public bool Accessible { get; set; }
    public double EntryFee { get; set; }
    public string OpeningHours { get; set; }
    public DateTime CreatedAt { get; set; }

    // navegación 1-N
    public ICollection<Trail> Trails { get; set; }
    public ICollection<Photo> Photos { get; set; }
    public ICollection<Review> Reviews { get; set; }

    //navegación  nn-n con amenity
    public ICollection<PlaceAmenity> PlaceAmenities { get; set; }
}