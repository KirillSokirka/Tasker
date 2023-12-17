using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Interfaces.Queries;

public interface IGetUserRolesQuery
{
    Task<IList<string>> ExecuteAsync(ApplicationUser user);
}