using System.ComponentModel.DataAnnotations;

namespace NatureAPI.Models;

public class PhotoCreateDto
{
    [Required, StringLength(500)]
    public string Url { get; set; } = null!;

    [StringLength(300)]
    public string? Description { get; set; }
}