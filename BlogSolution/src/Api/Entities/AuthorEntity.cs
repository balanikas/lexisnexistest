using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.Entities;

public class AuthorEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int BlogCount { get; set; }
}