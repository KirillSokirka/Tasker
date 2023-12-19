using System.Linq.Expressions;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Domain.Entities.Application;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Application.Interfaces.Resolvers;

public interface IProjectResolver
{
    Task<Project> ResolveAsync(string id);

    Task ResolveAdminProjectsAsync(
        Expression<Func<AdminProjectUser, bool>> predicate, List<UserProjectDto>? userProjectDto);

    Task ResolveAssignedProjectsAsync(
        Expression<Func<AssignedProjectUser, bool>> predicate, List<UserProjectDto>? userProjectDto);
}