using Tasker.Application.DTOs;

namespace Tasker.Domain.Models.Identity;

public class RefreshTokenModel : TokenModel
{
    public string Email { get; set; } = null!;
}