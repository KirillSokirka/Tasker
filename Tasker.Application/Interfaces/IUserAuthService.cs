using Microsoft.AspNetCore.Identity;
using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Auth;
using Tasker.Domain.Models.Application;
using Tasker.Domain.Models.Identity;

namespace Tasker.Application.Interfaces;

public interface IUserAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    Task<OperationResult> LoginUserAsync(LoginModel model);
    Task<OperationResult> UpdatePasswordAsync(PasswordUpdateModel model);
}