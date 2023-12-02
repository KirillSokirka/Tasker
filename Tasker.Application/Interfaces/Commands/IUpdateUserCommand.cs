using Microsoft.AspNetCore.Identity;
using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Interfaces.Commands;

public interface IUpdateUserCommand 
{
    Task<IdentityResult> ExecuteAsync(ApplicationUser user);
}