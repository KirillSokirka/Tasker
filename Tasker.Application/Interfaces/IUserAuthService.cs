using Microsoft.AspNetCore.Identity;
using Tasker.Application.DTOs;

namespace Tasker.Application.Interfaces;

public interface IUserAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    Task<LoginOperationResult> LoginUserAsync(LoginModel model);
}