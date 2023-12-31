namespace Tasker.Application.DTOs.Application.TaskStatus;

public class TaskStatusUpdateDto
{
    public required string Id { get; set; }
    public string? Name { get; set; }
    public int Order { get; set; }
    public required string KanbanBoardId { get; set; }
}