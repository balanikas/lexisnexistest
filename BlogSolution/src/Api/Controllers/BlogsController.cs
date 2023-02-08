using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Api.Controllers;

[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/[controller]")]
public class BlogsController : ControllerBase
{
    private readonly IAuthorsRepository _authorsRepository;
    private readonly IBlogsRepository _blogsRepository;
    private readonly ILogger _logger;
    private readonly ITransactionHandler _transactionHandler;

    public BlogsController(
        IBlogsRepository blogsRepository,
        IAuthorsRepository authorsRepository,
        ITransactionHandler transactionHandler,
        ILogger<BlogsController> logger)
    {
        _blogsRepository = blogsRepository;
        _authorsRepository = authorsRepository;
        _transactionHandler = transactionHandler;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BlogModel>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            _logger.LogError("Invalid blog id format for id {Id}", id);
            return BadRequest("Invalid blog id format");
        }

        var blog = await _blogsRepository.Get(id);
        if (blog is null) return NotFound($"Blog with id {id} was not found");

        var author = await _authorsRepository.Get(blog.AuthorId);
        if (author is null) return NotFound($"Author with id {blog.AuthorId} was not found");

        return Mapping.ToBlogModel(blog, author);
    }

    [HttpPost]
    public async Task<IActionResult> Post(BlogInputModel model)
    {
        if (!ObjectId.TryParse(model.AuthorId, out _))
        {
            _logger.LogError("Invalid author id format for id {Id}", model.AuthorId);
            return BadRequest("Invalid author id format");
        }

        var author = await _authorsRepository.Get(model.AuthorId);
        if (author is null)
        {
            _logger.LogError("Author with id {AuthorId} was not found", model.AuthorId);
            return BadRequest($"Author with id {model.AuthorId} was not found");
        }

        var blogEntity = await _blogsRepository.Create(Mapping.ToBlogEntity(model));
        author.BlogCount++;
        await _authorsRepository.Update(author.Id, author);
        var blog = Mapping.ToBlogModel(blogEntity, author);

        return CreatedAtAction(nameof(Get), new { id = blog.Id }, blog);
    }
}