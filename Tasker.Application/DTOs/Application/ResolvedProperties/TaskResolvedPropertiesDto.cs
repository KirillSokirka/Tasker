namespace Tasker.Application.DTOs.Application.ResolvedProperties;

public class TaskResolvedPropertiesDto
{
    public Domain.Entities.Application.TaskStatus? Status { get; set; }
    public Domain.Entities.Application.Release? Release { get; set; }
    public Domain.Entities.Application.User? Assignee { get; set; }
}