using System.Security.Cryptography;
using Tasker.Application.DTOs;
using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(ApplicationUser user);
    public string GenerateRefreshToken();
    Task<RefreshTokenModel> RefreshToken(RefreshTokenModel tokenModel);
}