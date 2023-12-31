using Tasker.Domain.Enums;

namespace Tasker.Application.DTOs.Application.Task;

public class TaskPreviewDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string TaskStatusName { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.None;
    public string Assignee { get; set; }
}