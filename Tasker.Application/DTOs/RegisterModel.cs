using System.ComponentModel.DataAnnotations;

namespace Tasker.Application.DTOs;

public class RegisterModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    
    [Required]
    public string Username { get; set; } = null!;
}