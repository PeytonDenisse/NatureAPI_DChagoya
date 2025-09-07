namespace NatureAPI.Models.Entities;

public class Photo
{
    public int Id { get; set; }
    public int PlaceId { get; set; } // FK
    public string Url { get; set; }

    // navegación 
    public Place Place { get; set; }
}