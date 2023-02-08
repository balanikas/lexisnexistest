using System.Net;
using Api.Models;
using FluentAssertions;
using FluentAssertions.Http;
using MongoDB.Bson;

namespace Api.IntegrationTests;

public class Blogs : IClassFixture<Factory<Program>>
{
    private readonly Factory<Program> _factory;

    public Blogs(Factory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_WhenBlogDoesNotExists_ShouldReturn404()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"api/blogs/{ObjectId.GenerateNewId()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_WhenUnsupportedIdIsProvided_ShouldReturn400()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"api/blogs/{123}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("text", " ")]
    [InlineData("text", "invalidauthorid")]
    [InlineData("text", null)]
    public async Task Post_WithInvalidPayload_ShouldReturn400(string text, string authorId)
    {
        var client = _factory.CreateClient();

        var payload = new BlogInputModel
        {
            Text = text,
            AuthorId = authorId,
            CreatedOn = DateTime.UtcNow,
            PublishedOn = DateTime.UtcNow
        };
        var content = Shared.Serialize(payload);
        var response = await client.PostAsync("api/blogs", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_ShouldCreateBlog()
    {
        var client = _factory.CreateClient();
        var response = await Shared.CreateAuthor(client);
        var author = await Shared.Deserialize<AuthorModel>(response);
        var payload = new BlogInputModel
        {
            Text = "text",
            AuthorId = author.Id,
            CreatedOn = DateTime.UtcNow,
            PublishedOn = DateTime.UtcNow
        };

        response = await Shared.CreateBlog(client, payload);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Should().HaveContentHeaderValue(HttpResponseHeader.ContentType, "application/json; charset=utf-8");

        var blog = await Shared.Deserialize<BlogModel>(response);
        response.Should().HaveContent(blog, o =>
            o.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .WhenTypeIs<DateTime>());

        blog.Author.BlogCount.Should().Be(1);
    }

    [Fact]
    public async Task Get_ShouldReturnBlog()
    {
        var client = _factory.CreateClient();
        var response = await Shared.CreateAuthor(client);
        var author = await Shared.Deserialize<AuthorModel>(response);
        var blog = await Shared.CreateBlog(client, author.Id);

        response = await client.GetAsync($"api/blogs/{blog.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().HaveContentHeaderValue(HttpResponseHeader.ContentType, "application/json; charset=utf-8");
        response.Should().HaveContent(blog, o =>
            o.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .WhenTypeIs<DateTime>());
    }
}