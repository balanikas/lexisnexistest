using Api.Entities;
using MongoDB.Driver;

namespace Api.Services;

internal class BlogsRepository : IBlogsRepository
{
    private readonly IDatabaseContext _context;

    public BlogsRepository(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<BlogEntity?> Get(string id)
    {
        return await _context.GetBlogsCollection().Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<BlogEntity>> GetByAuthor(string authorId)
    {
        return await _context.GetBlogsCollection().Find(x => x.AuthorId == authorId).ToListAsync();
    }

    public async Task<BlogEntity> Create(BlogEntity blog)
    {
        await _context.GetBlogsCollection().InsertOneAsync(blog);
        return blog;
    }
}