using System.ComponentModel.DataAnnotations;

namespace NatureAPI.Models;

public class PlaceCreateDto
{
    [Required, StringLength(120)]
    public string Name { get; set; } = null!;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required, StringLength(40)]
    public string Category { get; set; } = null!;

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    [Range(-500, 9000)]
    public int ElevationMeters { get; set; }

    public bool Accessible { get; set; }

    [Range(0, double.MaxValue)]
    public double EntryFee { get; set; }

    [StringLength(80)]
    public string? OpeningHours { get; set; }

    //  NUEVO
    public List<PhotoCreateDto>? Photos { get; set; }
}