using System.ComponentModel.DataAnnotations;

namespace NatureAPI.Models.Entities;

public class Amenity
{
    public int Id { get; set; }

    [Required, StringLength(60)]
    public string Name { get; set; } = null!;

    // navegación
    public ICollection<PlaceAmenity> PlaceAmenities { get; set; } = new List<PlaceAmenity>();
}