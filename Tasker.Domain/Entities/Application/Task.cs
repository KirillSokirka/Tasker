using Tasker.Domain.Enums;

namespace Tasker.Domain.Entities.Application;

public class Task
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    public string ProjectId { get; set; } = null!;
    public Project Project { get; set; } = null!;
    
    public string? TaskStatusId { get; set; }
    public TaskStatus? Status { get; set; } = null;
    
    public string? ReleaseId { get; set; } = null;
    public Release? Release { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.None;
    public DateTime CreationDate { get; set; }
    
    public string? AssigneeId { get; set; }
    public User? Assignee { get; set; } = null;
    
    public string CreatorId { get; set; } = null!;
    public User Creator { get; set; } = null!;
}