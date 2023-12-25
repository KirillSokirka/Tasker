using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tasker.Application.Interfaces;
using Tasker.Application.Interfaces.Commands;
using Tasker.Application.Interfaces.Queries;
using Tasker.Domain.Entities.Identity;
using Tasker.Domain.Models.Identity;

namespace Tasker.Application.Services;

public class TokenService : ITokenService
{
    private readonly IFindUserByNameQuery _findUserByNameQuery;
    private readonly IGetUserRolesQuery _getUserRolesQuery;
    private readonly IUpdateUserCommand _updateUserCommand;
    private readonly IFindByIdQuery _findByIdQuery;
    private readonly IConfiguration _configuration;
    
    public TokenService(IConfiguration configuration, IFindUserByNameQuery findUserByNameQuery,
        IUpdateUserCommand updateUserCommand, IFindByIdQuery byIdQuery, 
        IGetUserRolesQuery getUserRolesQuery)
    {
        _configuration = configuration;
        _findUserByNameQuery = findUserByNameQuery;
        _updateUserCommand = updateUserCommand;
        _findByIdQuery = byIdQuery;
        _getUserRolesQuery = getUserRolesQuery;
    }

    public async Task<TokenModel> GenerateTokensPairAsync(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var secretKey = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = await GenerateUserClaims(user);
        
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var token = tokenHandler.CreateToken(descriptor);
        
        var refreshToken = GenerateRefreshToken();

        var saveResult = await SaveRefreshTokenAsync(user.Id, refreshToken);
        
        if (saveResult is false)
        {
            throw new AuthenticationException("Invalid client request");
        }
        
        return new TokenModel
        {
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken
        };
    }
    
    public async Task<TokenModel> RefreshTokenAsync(RefreshTokenModel tokenModel)
    {
        var principal = GetPrincipalFromExpiredToken(tokenModel.Token);

        var username =
            principal.Identity?.Name ??
            throw new AuthenticationException("Invalid client request");

        var user = await _findUserByNameQuery.ExecuteAsync(username);
        
        if (user is null ||
            user.RefreshToken != tokenModel.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new AuthenticationException("Invalid client request");
        }

        var newTokens = await GenerateTokensPairAsync(user);
        
        return newTokens;
    }

    #region Private Methods

    private async Task<List<Claim>> GenerateUserClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var roles = await _getUserRolesQuery.ExecuteAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
    
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ??
                                         throw new KeyNotFoundException("Key should be specified"));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
    
    private async Task<bool> SaveRefreshTokenAsync(string userId, string refreshToken)
    {
        var user = await _findByIdQuery.ExecuteAsync(userId);

        if (user == null) return false;

        user.RefreshToken = refreshToken;
        
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        var result = await _updateUserCommand.ExecuteAsync(user);

        return result.Succeeded;
    }

    #endregion
}