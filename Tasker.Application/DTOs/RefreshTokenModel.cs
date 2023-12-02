namespace Tasker.Application.DTOs;

public class RefreshTokenModel
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!; 
    public string Email { get; set; } = null!;
}