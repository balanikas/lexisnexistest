using System.Net;
using Api.Models;
using FluentAssertions;
using FluentAssertions.Http;
using MongoDB.Bson;

namespace Api.IntegrationTests;

public class Authors : IClassFixture<Factory<Program>>
{
    private readonly Factory<Program> _factory;

    public Authors(Factory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_WhenAuthorDoesNotExists_ShouldReturn404()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"api/authors/{ObjectId.GenerateNewId()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_WhenUnsupportedIdIsProvided_ShouldReturn400()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"api/authors/{123}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData(null, "last")]
    [InlineData("first", null)]
    public async Task Post_WithInvalidPayload_ShouldReturn400(string firstName, string lastName)
    {
        var client = _factory.CreateClient();

        var payload = new AuthorInputModel { FirstName = firstName, LastName = lastName };
        var content = Shared.Serialize(payload);
        var response = await client.PostAsync("api/authors", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_WhenAuthorIsCreated_ShouldReturnAuthor()
    {
        var client = _factory.CreateClient();

        var response = await Shared.CreateAuthor(client);
        var author = await Shared.Deserialize<AuthorModel>(response);
        var resourcePath = response.Headers.Location?.AbsolutePath;
        response = await client.GetAsync(resourcePath);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().HaveContent(author);
        response.Should().HaveContentHeaderValue(HttpResponseHeader.ContentType, "application/json; charset=utf-8");
    }

    [Fact]
    public async Task WhenAuthorHaveBlogs_ShouldIncludeBlogCountInResponse()
    {
        var client = _factory.CreateClient();
        var response = await Shared.CreateAuthor(client);
        var author = await Shared.Deserialize<AuthorModel>(response);
        author.BlogCount.Should().Be(0);

        response = await client.GetAsync($"api/authors/{author.Id}");
        author = await Shared.Deserialize<AuthorModel>(response);
        author.BlogCount.Should().Be(0);

        await Shared.CreateBlog(client, (BlogInputModel)new()
        {
            Text = "text",
            AuthorId = author.Id
        });

        response = await client.GetAsync($"api/authors/{author.Id}");
        author = await Shared.Deserialize<AuthorModel>(response);
        author.BlogCount.Should().Be(1);
    }
    
    [Fact]
    public async Task WhenAuthorHaveBlogs_ShouldReturnBlogs()
    {
        var client = _factory.CreateClient();
        var response = await Shared.CreateAuthor(client);
        var author = await Shared.Deserialize<AuthorModel>(response);
       
        await Shared.CreateBlog(client, (BlogInputModel)new()
        {
            Text = "text1",
            AuthorId = author.Id
        });
        
        await Shared.CreateBlog(client, (BlogInputModel)new()
        {
            Text = "text2",
            AuthorId = author.Id
        });

        response = await client.GetAsync($"api/authors/{author.Id}/blogs");
        var blogs = await Shared.Deserialize<List<BlogModel>>(response);
        blogs.Count.Should().Be(2);
    }
}