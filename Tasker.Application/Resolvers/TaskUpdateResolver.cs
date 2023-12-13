using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.Resolvers.DTOs;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.Resolvers;

public class TaskUpdateResolver : IResolver<TaskResolvedPropertiesDto, TaskUpdateDto>
{
    private readonly IResolver<User, string> _userResolver;
    private readonly IResolver<Project, string> _projectResolver;
    private readonly IResolver<Release, string> _releaseResolver;
    private readonly IResolver<TaskStatus, string> _statusResolver;

    public TaskUpdateResolver(IResolver<User, string> userResolver,
        IResolver<Project, string> projectResolver,
        IResolver<Release, string> releaseResolver,
        IResolver<TaskStatus, string> statusResolver)
    {
        _userResolver = userResolver;
        _projectResolver = projectResolver;
        _releaseResolver = releaseResolver;
        _statusResolver = statusResolver;
    }

    public async Task<TaskResolvedPropertiesDto> ResolveAsync(TaskUpdateDto dto)
    {
        var result = new TaskResolvedPropertiesDto
        {
            Assignee = dto.Assignee?.Id is not null
                ? await _userResolver.ResolveAsync(dto.Assignee.Id)
                : null,
            Status = dto.Status?.Id is not null
                ? await _statusResolver.ResolveAsync(dto.Status.Id)
                : null,
            Release = dto.Release?.Id is not null
                ? await _releaseResolver.ResolveAsync(dto.Release.Id)
                : null
        };

        return result;
    }
}