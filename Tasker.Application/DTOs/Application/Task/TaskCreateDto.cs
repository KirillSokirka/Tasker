using Tasker.Domain.Enums;

namespace Tasker.Application.DTOs.Application.Task
{
    public class TaskCreateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ProjectId { get; set; }
        public string? TaskStatusId { get; set; }
        public string? ReleaseId { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.None;
        public DateTime CreationDate { get; set; }
        public string? AssigneeId { get; set; }
        public string CreatorId { get; set; } = null!;
    }
}
