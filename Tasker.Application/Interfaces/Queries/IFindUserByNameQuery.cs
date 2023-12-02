using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Interfaces.Queries;

public interface IFindUserByNameQuery
{
    Task<ApplicationUser?> ExecuteAsync(string username);
}