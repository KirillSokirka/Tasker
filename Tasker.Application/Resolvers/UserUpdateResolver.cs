using Tasker.Application.DTOs.Application.User;
using Tasker.Application.Resolvers.DTOs;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Application.Resolvers;

public class UserUpdateResolver : IResolver<UserResolvedPropertiesDto, UserUpdateDto>
{
    private readonly IResolver<Project, string> _resolver;

    public UserUpdateResolver(IResolver<Project, string> resolver)
    {
        _resolver = resolver;
    }

    public Task<UserResolvedPropertiesDto> ResolveAsync(UserUpdateDto dto)
        => Task.FromResult(new UserResolvedPropertiesDto
        {
            AssignedProjects = dto.AssignedProjects?
                .Select(project => _resolver.ResolveAsync(project).Result)
                .ToList(),
            UnderControlProjects = dto.UnderControlProjects?
                .Select(project => _resolver.ResolveAsync(project).Result)
                .ToList()
        });
}