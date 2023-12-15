using System.ComponentModel.DataAnnotations;

namespace Tasker.Domain.Models.Identity;

public class LoginModel
{
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}