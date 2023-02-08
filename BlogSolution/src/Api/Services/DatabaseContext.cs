using Api.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api.Services;

public class DatabaseContext : IDatabaseContext
{
    private readonly IMongoDatabase _database;
    private readonly DatabaseSettings _settings;

    public DatabaseContext(IOptions<DatabaseSettings> settings)
    {
        _settings = settings.Value;
        Client = new MongoClient(_settings.ConnectionString);
        _database = Client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoClient Client { get; }

    public IMongoCollection<AuthorEntity> GetAuthorsCollection()
    {
        return _database.GetCollection<AuthorEntity>(_settings.AuthorsCollectionName);
    }

    public IMongoCollection<BlogEntity> GetBlogsCollection()
    {
        return _database.GetCollection<BlogEntity>(_settings.BlogsCollectionName);
    }
}