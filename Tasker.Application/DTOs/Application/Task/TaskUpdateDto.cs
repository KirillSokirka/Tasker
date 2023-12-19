using Tasker.Domain.Enums;

namespace Tasker.Application.DTOs.Application.Task;

public class TaskUpdateDto
{
    public required string Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? StatusId { get; set; } // required for correct update 
    public string? ReleaseId { get; set; }
    public string? AssigneeId { get; set; }
    public TaskPriority? Priority { get; set; }
}