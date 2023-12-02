using Microsoft.AspNetCore.Identity;
using Tasker.Application.Interfaces.Queries;
using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Queries;

public class FindUserByIdQuery : IFindByIdQuery
{
    private readonly UserManager<ApplicationUser> _userManager;

    public FindUserByIdQuery(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> ExecuteAsync(string username)
        => await _userManager.FindByIdAsync(username) ?? null;
}