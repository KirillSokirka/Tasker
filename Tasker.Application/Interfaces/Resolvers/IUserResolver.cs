using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.ResolvedProperties;
using Tasker.Application.DTOs.Application.User;
using Tasker.Domain.Entities.Application;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Application.Interfaces.Resolvers;

public interface IUserResolver : IResolver<User, string>
{
    Task ResolveAsync(List<UserProjectDto>? assigned,
        List<UserProjectDto>? admin,
        string userId);
}