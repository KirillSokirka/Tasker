using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.Resolvers.DTOs;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;

namespace Tasker.Application.Resolvers;

public class TaskResolver : IResolver<TaskResolvedPropertiesDto, TaskUpdateDto>
{
    private readonly IResolver<User, UserDto> _userResolver;
    private readonly IResolver<Project, ProjectDto> _projectResolver;

    public TaskResolver(IResolver<User, UserDto> userResolver,
        IResolver<Project, ProjectDto> projectResolver)
    {
        _userResolver = userResolver;
        _projectResolver = projectResolver;
    }

    public async Task<TaskResolvedPropertiesDto> ResolveAsync(TaskUpdateDto dto)
    {
        var result = new TaskResolvedPropertiesDto
        {
            Creator = dto.Creator is not null
                ? await _userResolver.ResolveAsync(dto.Creator)
                : null,
            Assignee = dto.Assignee is not null
                ? await _userResolver.ResolveAsync(dto.Assignee)
                : null,
            Project = dto.Project is not null
                ? await _projectResolver.ResolveAsync(dto.Project)
                : null
        };

        return result;
    }
}