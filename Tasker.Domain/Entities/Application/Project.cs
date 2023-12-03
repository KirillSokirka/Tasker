namespace Tasker.Domain.Entities.Application;

public class Project
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = null!;
    public List<KanbanBoard> KanbanBoards { get; set; } = new();
    public List<Task> Tasks { get; set; } = new();
    public List<Release> Releases { get; set; } = new();
}