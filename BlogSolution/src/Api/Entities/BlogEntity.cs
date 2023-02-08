using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.Entities;

public class BlogEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string AuthorId { get; set; } = null!;

    public DateTime PublishedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public string Text { get; set; } = null!;
}