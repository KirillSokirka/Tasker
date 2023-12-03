namespace Tasker.Domain.Entities.Application;

public class KanbanBoard
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = null!;
    
    public string ProjectId { get; set; } = null!;
    public Project Project { get; set; } = null!;
    
    public List<TaskStatus> Columns { get; set; } = null!;
}