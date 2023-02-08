using Api.Entities;
using Api.Models;

namespace Api.Services;

public static class Mapping
{
    public static AuthorEntity ToAuthorEntity(AuthorInputModel model)
    {
        return new()
        {
            FirstName = model.FirstName,
            LastName = model.LastName
        };
    }

    public static AuthorModel ToAuthorModel(AuthorEntity authorEntity)
    {
        return new()
        {
            Id = authorEntity.Id,
            FirstName = authorEntity.FirstName,
            LastName = authorEntity.LastName,
            BlogCount = authorEntity.BlogCount
        };
    }

    public static BlogEntity ToBlogEntity(BlogInputModel model)
    {
        return new()
        {
            AuthorId = model.AuthorId,
            Text = model.Text,
            PublishedOn = model.PublishedOn,
            CreatedOn = model.CreatedOn
        };
    }

    public static BlogModel ToBlogModel(BlogEntity blogEntity, AuthorEntity authorEntity)
    {
        return new()
        {
            Id = blogEntity.Id,
            CreatedOn = blogEntity.CreatedOn,
            PublishedOn = blogEntity.PublishedOn,
            Text = blogEntity.Text,
            Author = ToAuthorModel(authorEntity)
        };
    }
}