namespace Tasker.Domain.Entities.Application;

public class Release
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = null!;
    public bool IsReleased { get; set; }
    
    public DateTime CreationDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public string ProjectId { get; set; } = null!;
    public Project Project { get; set; } = null!;
    
    public List<Task> Tasks { get; set; } = new();
}