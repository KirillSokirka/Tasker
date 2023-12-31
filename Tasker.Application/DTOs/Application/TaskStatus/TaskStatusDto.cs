using Tasker.Application.DTOs.Application.Task;

namespace Tasker.Application.DTOs.Application.TaskStatus;

public class TaskStatusDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public int Order { get; set; }
    public string? KanbanBoardId { get; set; }
    public List<TaskPreviewDto>? Tasks { get; set; }
}