using System.Linq.Expressions;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Domain.Entities.Application;

namespace Tasker.Application.Interfaces.Resolvers;

public interface IProjectResolver
{
    Task<Project> ResolveAsync(string id);

    Task<List<AdminProjectUser>> ResolveAdminProjectsAsync(
        Expression<Func<AdminProjectUser, bool>> predicate, List<UserProjectDto>? userProjectDto);

    Task<List<AssignedProjectUser>> ResolveAssignedProjectsAsync(
        Expression<Func<AssignedProjectUser, bool>> predicate, List<UserProjectDto>? userProjectDto);
}