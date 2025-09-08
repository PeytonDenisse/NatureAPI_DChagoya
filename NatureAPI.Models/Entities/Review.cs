using System.ComponentModel.DataAnnotations;

namespace NatureAPI.Models.Entities;

public class Review
{
    public int Id { get; set; }
    public int PlaceId { get; set; }    // FK

    [Required, StringLength(120)]
    public string Author { get; set; } = null!;

    [Range(1,5)]
    public int Rating { get; set; }

    [StringLength(2000)]
    public string? Comment { get; set; }  // opcional

    public DateTime CreatedAt { get; set; }

    // navegación
    public Place Place { get; set; } = null!;
}