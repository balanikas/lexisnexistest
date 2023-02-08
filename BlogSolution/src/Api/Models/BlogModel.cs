namespace Api.Models;

public class BlogModel
{
    public string? Id { get; set; }

    public AuthorModel Author { get; set; } = null!;

    public DateTime PublishedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public string Text { get; set; } = null!;
}