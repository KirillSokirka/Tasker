using Microsoft.AspNetCore.Identity;
using Tasker.Application.DTOs.Auth;
using Tasker.Domain.Entities.Identity;
using Tasker.Domain.Models.Identity;

namespace Tasker.Application.Interfaces.Services;

public interface IUserAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    Task<OperationResult> LoginUserAsync(LoginModel model);
    Task<OperationResult> UpdatePasswordAsync(PasswordUpdateModel model);
    Task<OperationResult> UpdateUserRolesAsync(UpdateUserRoleModel model);
    Task DeleteUserAsync(string userId);
}