using Microsoft.AspNetCore.Identity;
using Tasker.Application.Interfaces.Queries;
using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Queries;

public class GetUserRolesQuery : IGetUserRolesQuery
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserRolesQuery(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IList<string>> ExecuteAsync(ApplicationUser user)
        => await _userManager.GetRolesAsync(user);
}