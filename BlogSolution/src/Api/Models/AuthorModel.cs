namespace Api.Models;

public class AuthorModel
{
    public string? Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int BlogCount { get; set; }
}