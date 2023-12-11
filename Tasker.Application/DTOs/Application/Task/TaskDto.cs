using Tasker.Domain.Enums;

namespace Tasker.Application.DTOs.Application.Task;

public class TaskDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Title { get; set; }
    public required ProjectDto Project { get; set; }
    public required UserDto Creator { get; set; }
    public string Description { get; set; } = null!;
    public TaskStatusDto? Status { get; set; }
    public ReleaseDto? Release { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.None;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public UserDto? Assignee { get; set; }
}