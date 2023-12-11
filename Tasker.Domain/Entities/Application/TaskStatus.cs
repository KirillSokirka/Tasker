namespace Tasker.Domain.Entities.Application;

public class TaskStatus
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string KanbanBoardId { get; set; } = null!; 
    public KanbanBoard KanbanBoard { get; set; } = null!; 
    public List<Task> Tasks { get; set; } = new();
}