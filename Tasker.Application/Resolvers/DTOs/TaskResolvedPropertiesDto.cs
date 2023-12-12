using Tasker.Domain.Entities.Application;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.Resolvers.DTOs;

public class TaskResolvedPropertiesDto
{
    public Project? Project { get; set; }
    public User? Creator { get; set; }
    public TaskStatus? Status { get; set; }
    public Release? Release { get; set; }
    public User? Assignee { get; set; }
}