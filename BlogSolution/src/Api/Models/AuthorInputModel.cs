using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class AuthorInputModel
{
    [Required] public string FirstName { get; set; } = null!;

    [Required] public string LastName { get; set; } = null!;
}