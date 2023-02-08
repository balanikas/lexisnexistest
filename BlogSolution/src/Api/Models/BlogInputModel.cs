using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class BlogInputModel
{
    [Required] public string? AuthorId { get; set; }

    public DateTime PublishedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    [Required] public string Text { get; set; } = null!;
}