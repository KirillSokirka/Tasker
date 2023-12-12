using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.Resolvers.DTOs;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;

namespace Tasker.Application.Resolvers;

public class TaskResolver : IResolver<TaskResolvedPropertiesDto, TaskUpdateDto>
{
    private readonly IResolver<User, string> _userResolver;
    private readonly IResolver<Project, string> _projectResolver;
    private readonly IResolver<Release, string> _releaseResolver;

    public TaskResolver(IResolver<User, string> userResolver,
        IResolver<Project, string> projectResolver,
        IResolver<Release, string> releaseResolver)
    {
        _userResolver = userResolver;
        _projectResolver = projectResolver;
        _releaseResolver = releaseResolver;
    }

    public async Task<TaskResolvedPropertiesDto> ResolveAsync(TaskUpdateDto dto)
    {
        var result = new TaskResolvedPropertiesDto
        {
            Creator = dto.Creator?.Id is not null
                ? await _userResolver.ResolveAsync(dto.Creator.Id)
                : null,
            Assignee = dto.Assignee?.Id is not null
                ? await _userResolver.ResolveAsync(dto.Assignee.Id)
                : null,
            Project = dto.Project?.Id is not null
                ? await _projectResolver.ResolveAsync(dto.Project.Id)
                : null,
            Release = dto.Release?.Id is not null
                ? await _releaseResolver.ResolveAsync(dto.Release.Id)
                : null
        };

        return result;
    }
}