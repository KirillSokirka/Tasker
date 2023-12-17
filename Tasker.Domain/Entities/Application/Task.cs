using Tasker.Domain.Enums;

namespace Tasker.Domain.Entities.Application;

public class Task
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Title { get; set; }
    public string? Description { get; set; }
    
    public string? ProjectId { get; set; }
    public Project? Project { get; set; }

    public string? TaskStatusId { get; set; }
    public TaskStatus? Status { get; set; }
    
    public string? ReleaseId { get; set; }
    public Release? Release { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.None;
    public DateTime CreationDate { get; set; }
    
    public string? AssigneeId { get; set; }
    public User? Assignee { get; set; }
    
    public string CreatorId { get; set; } = null!;
    public User Creator { get; set; } = null!;
}