using Api.Entities;

namespace Api.Services;

public interface IBlogsRepository
{
    Task<BlogEntity?> Get(string id);
    Task<IEnumerable<BlogEntity>> GetByAuthor(string authorId);
    Task<BlogEntity> Create(BlogEntity blog);
}