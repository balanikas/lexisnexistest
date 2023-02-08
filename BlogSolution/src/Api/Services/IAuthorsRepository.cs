using Api.Entities;

namespace Api.Services;

public interface IAuthorsRepository
{
    Task<AuthorEntity?> Get(string id);
    Task<IEnumerable<AuthorEntity>> Get();
    Task Update(string id, AuthorEntity entity);

    Task<AuthorEntity> Create(AuthorEntity authorEntity);
}