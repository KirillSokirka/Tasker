using System.Security.Cryptography;
using Tasker.Application.DTOs;
using Tasker.Domain.Entities.Identity;
using Tasker.Domain.Models.Identity;

namespace Tasker.Application.Interfaces;

public interface ITokenService
{
    Task<TokenModel> GenerateTokensPairAsync(ApplicationUser user);
    Task<TokenModel> RefreshTokenAsync(RefreshTokenModel tokenModel);
}