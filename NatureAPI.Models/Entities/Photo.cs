using System.ComponentModel.DataAnnotations;

namespace NatureAPI.Models.Entities;

public class Photo
{
    public int Id { get; set; }
    public int PlaceId { get; set; }   // FK

    [Required, StringLength(500)]
    public string Url { get; set; } = null!;

    [StringLength(300)]
    public string? Description { get; set; }   // opcional

    // navegación
    public Place Place { get; set; } = null!;
}