using Microsoft.IdentityModel.Tokens;
using Tasker.Application.DTOs.Application.ResolvedProperties;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Domain.Repositories;
using Task = Tasker.Domain.Entities.Application.Task;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.Resolvers;

public class TaskResolver : ITaskResolver
{
    private readonly IResolver<TaskStatus, string> _statusResolver;
    private readonly IProjectResolver _projectResolver;
    private readonly IResolver<Release, string> _releaseResolver;
    private readonly IEntityRepository<Task> _taskRepository;
    private readonly IUserResolver _userResolver;
    
    public TaskResolver(IEntityRepository<Task> taskRepository, IUserResolver userResolver,
        IResolver<Release, string> releaseResolver, IResolver<TaskStatus, string> statusResolver,
        IProjectResolver projectResolver)
    {
        _userResolver = userResolver;
        _releaseResolver = releaseResolver;
        _statusResolver = statusResolver;
        _taskRepository = taskRepository;
        _projectResolver = projectResolver;
    }

    public async Task<Task> ResolveAsync(string id) =>
        await _taskRepository.GetByIdAsync(id)
        ?? throw new InvalidEntityException($"Task with id {id} doesnt exists");

    public async Task<Task> ResolveAsync(TaskCreateDto dto) =>
        new()
        {
            Title = dto.Title,
            Description = dto.Description,
            Creator = await _userResolver.ResolveAsync(dto.CreatorId),
            CreatorId = dto.CreatorId,
            Project = await _projectResolver.ResolveAsync(dto.ProjectId),
            ProjectId = dto.ProjectId,
            Release = dto.ReleaseId is null ? null : await _releaseResolver.ResolveAsync(dto.ReleaseId),
            Status = dto.TaskStatusId is null ? null : await _statusResolver.ResolveAsync(dto.TaskStatusId),
            Assignee = dto.AssigneeId is null ? null : await _userResolver.ResolveAsync(dto.AssigneeId)
        };

    public async Task<TaskResolvedPropertiesDto> ResolveAsync(TaskUpdateDto dto)
    {
        var result = new TaskResolvedPropertiesDto
        {
            Assignee = !string.IsNullOrEmpty(dto.AssigneeId)
                ? await _userResolver.ResolveAsync(dto.AssigneeId)
                : null,
            Status = !string.IsNullOrEmpty(dto.StatusId)
                ? await _statusResolver.ResolveAsync(dto.StatusId)
                : null,
            Release = !string.IsNullOrEmpty(dto.ReleaseId)
                ? await _releaseResolver.ResolveAsync(dto.ReleaseId)
                : null
        };

        return result;
    }
    
    public async Task<TaskResolvedPropertiesDto> ResolveStatusAsync(TaskUpdateStatusDto dto)
    {
        var result = new TaskResolvedPropertiesDto
        {
            Status = !string.IsNullOrEmpty(dto.StatusId)
                ? await _statusResolver.ResolveAsync(dto.StatusId)
                : null,
        };

        return result;
    }
}