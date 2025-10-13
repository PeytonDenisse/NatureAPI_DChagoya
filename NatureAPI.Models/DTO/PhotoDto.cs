namespace NatureAPI.Models;

public class PhotoDto
{
    public int Id { get; init; }
    public string Url { get; init; } = null!;
    public string? Description { get; init; }

    public PhotoDto(int id, string url, string? description)
    {
        Id = id;
        Url = url;
        Description = description;
    }
}