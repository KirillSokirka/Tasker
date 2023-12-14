using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.User;
using Tasker.Domain.Enums;

namespace Tasker.Application.DTOs.Application.Task;

public class TaskDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Title { get; set; }
    public required string ProjectId { get; set; }
    public required UserDto Creator { get; set; }
    public string Description { get; set; } = null!;
    public string TaskStatusId { get; set; }
    public string ReleaseId { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.None;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public UserDto? Assignee { get; set; }
}