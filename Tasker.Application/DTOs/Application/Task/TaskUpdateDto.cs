using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.Release;
using Tasker.Application.DTOs.Application.TaskStatus;
using Tasker.Application.DTOs.Application.User;
using Tasker.Domain.Enums;

namespace Tasker.Application.DTOs.Application.Task;

public class TaskUpdateDto
{
    public required string Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatusDto? Status { get; set; }
    public ReleaseDto? Release { get; set; }
    public UserDto? Assignee { get; set; }
    public TaskPriority? Priority { get; set; }
}