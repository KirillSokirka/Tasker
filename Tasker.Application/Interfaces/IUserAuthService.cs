using Microsoft.AspNetCore.Identity;
using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Auth;

namespace Tasker.Application.Interfaces;

public interface IUserAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    Task<IdentityOperationResult> LoginUserAsync(LoginModel model);
    Task<IdentityOperationResult> UpdatePasswordAsync(PasswordUpdateModel model);
}