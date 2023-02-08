using Api.Entities;
using MongoDB.Driver;

namespace Api.Services;

public interface IDatabaseContext
{
    IMongoClient Client { get; }
    IMongoCollection<AuthorEntity> GetAuthorsCollection();
    IMongoCollection<BlogEntity> GetBlogsCollection();
}