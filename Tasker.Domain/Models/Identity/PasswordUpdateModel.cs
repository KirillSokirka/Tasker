namespace Tasker.Application.DTOs.Auth;

public class PasswordUpdateModel
{
    public required string Email { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}