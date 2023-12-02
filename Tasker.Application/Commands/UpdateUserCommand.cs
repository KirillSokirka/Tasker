using Microsoft.AspNetCore.Identity;
using Tasker.Application.Interfaces.Commands;
using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Commands;

public class UpdateUserCommand : IUpdateUserCommand
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserCommand(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> ExecuteAsync(ApplicationUser user)
        => await _userManager.UpdateAsync(user);
}