using Api.Entities;
using MongoDB.Driver;

namespace Api.Services;

internal class AuthorsRepository : IAuthorsRepository
{
    private readonly IDatabaseContext _context;

    public AuthorsRepository(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<AuthorEntity?> Get(string id)
    {
        return await _context.GetAuthorsCollection().Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<AuthorEntity>> Get()
    {
        return await _context.GetAuthorsCollection().Find(_ => true).ToListAsync();
    }

    public async Task Update(string id, AuthorEntity entity)
    {
        await _context.GetAuthorsCollection().ReplaceOneAsync(x => x.Id == id, entity);
    }

    public async Task<AuthorEntity> Create(AuthorEntity authorEntity)
    {
        await _context.GetAuthorsCollection().InsertOneAsync(authorEntity);
        return authorEntity;
    }
}