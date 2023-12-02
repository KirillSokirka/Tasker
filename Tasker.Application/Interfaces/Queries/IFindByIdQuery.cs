using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Interfaces.Queries;

public interface IFindByIdQuery
{
    Task<ApplicationUser?> ExecuteAsync(string id);
}