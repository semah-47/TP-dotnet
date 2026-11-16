using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ASPCoreFirstApp.Models;

public class MovieReview
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public int MovieId { get; set; }
    public string Author { get; set; } = string.Empty;
    public int Rating { get; set; } // 1 to 5
    public string? Comment { get; set; }
    public List<string> Tags { get; set; } = new(); // Flexible array (e.g., ["Sci-Fi", "MustWatch"])
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}