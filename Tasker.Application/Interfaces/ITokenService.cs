using System.Security.Cryptography;
using Tasker.Application.DTOs;
using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Interfaces;

public interface ITokenService
{
    Task<TokenModel> GenerateTokensPairAsync(ApplicationUser user);
    Task<TokenModel> RefreshTokenAsync(RefreshTokenModel tokenModel);
}