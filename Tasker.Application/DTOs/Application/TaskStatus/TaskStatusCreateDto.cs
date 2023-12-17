namespace Tasker.Application.DTOs.Application.TaskStatus;

public class TaskStatusCreateDto
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public required string KanbanBoardId { get; set; }
}