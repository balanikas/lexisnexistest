using System.Net;
using System.Text;
using System.Text.Json;
using Api.Models;
using FluentAssertions;

namespace Api.IntegrationTests;

public static class Shared
{
    public static async Task<HttpResponseMessage> CreateAuthor(HttpClient client)
    {
        var payload = new AuthorInputModel { FirstName = "first", LastName = "last" };
        var content = Serialize(payload);
        var response = await client.PostAsync("api/Authors", content);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return response;
    }


    public static async Task<HttpResponseMessage> CreateBlog(HttpClient client, BlogInputModel payload)
    {
        var content = Serialize(payload);
        var response = await client.PostAsync("api/blogs", content);
        return response;
    }


    public static async Task<BlogModel> CreateBlog(HttpClient client, string authorId)
    {
        var payload = new BlogInputModel
        {
            Text = "text",
            AuthorId = authorId,
            CreatedOn = DateTime.UtcNow,
            PublishedOn = DateTime.UtcNow
        };

        var content = Serialize(payload);
        var response = await client.PostAsync("api/blogs", content);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        return await Deserialize<BlogModel>(response);
    }

    public static async Task<T> Deserialize<T>(HttpResponseMessage response)
    {
        var contentAsString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(contentAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public static StringContent Serialize(object payload)
    {
        return new(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
    }
}