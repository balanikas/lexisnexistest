using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Api.Controllers;

[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorsRepository _authorsRepository;
    private readonly IBlogsRepository _blogsRepository;
    private readonly ILogger _logger;

    public AuthorsController(IAuthorsRepository authorsRepository, ILogger<AuthorsController> logger,
        IBlogsRepository blogsRepository)
    {
        _authorsRepository = authorsRepository;
        _blogsRepository = blogsRepository;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorModel>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            _logger.LogError("Invalid author id format for id {Id}", id);
            return BadRequest("Invalid author id format");
        }

        var entity = await _authorsRepository.Get(id);
        if (entity is null) return NotFound();

        return Mapping.ToAuthorModel(entity);
    }

    [HttpGet("{id}/blogs/{blogId}")]
    public async Task<ActionResult<BlogModel>> Get(string id, string blogId)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            _logger.LogError("Invalid author id format for id {Id}", id);
            return BadRequest("Invalid author id format");
        }

        if (!ObjectId.TryParse(blogId, out _))
        {
            _logger.LogError("Invalid blog id format for id {Id}", blogId);
            return BadRequest("Invalid blog id format");
        }

        var authorEntity = await _authorsRepository.Get(id);
        if (authorEntity is null) return NotFound();

        var blog = await _blogsRepository.Get(blogId);
        if (blog is null) return NotFound();

        return Mapping.ToBlogModel(blog, authorEntity);
    }


    [HttpGet("{id}/blogs")]
    public async Task<ActionResult<IEnumerable<BlogModel>>> GetBlogs(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            _logger.LogError("Invalid author id format for id {Id}", id);
            return BadRequest("Invalid author id format");
        }

        var entities = await _blogsRepository.GetByAuthor(id);
        var blogs = new List<BlogModel>();
        foreach (var blogEntity in entities)
        {
            var author = await _authorsRepository.Get(blogEntity.AuthorId);
            blogs.Add(Mapping.ToBlogModel(blogEntity, author));
        }

        return blogs;
    }

    [HttpPost]
    public async Task<IActionResult> Post(AuthorInputModel inputModel)
    {
        var authorEntity = await _authorsRepository.Create(Mapping.ToAuthorEntity(inputModel));
        var author = Mapping.ToAuthorModel(authorEntity);

        return CreatedAtAction(nameof(Get), new { id = author.Id }, author);
    }
}