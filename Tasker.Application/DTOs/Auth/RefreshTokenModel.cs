namespace Tasker.Application.DTOs;

public class RefreshTokenModel : TokenModel
{
    public string Email { get; set; } = null!;
}