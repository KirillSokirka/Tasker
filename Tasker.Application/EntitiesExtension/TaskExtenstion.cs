using Tasker.Application.DTOs.Application.ResolvedProperties;
using Tasker.Application.DTOs.Application.Task;
using Task = Tasker.Domain.Entities.Application.Task;

namespace Tasker.Application.EntitiesExtension;

public static class TaskExtenstion
{
    public static void Update(this Task task, TaskUpdateDto updateDto,
        TaskResolvedPropertiesDto resolvedProperties)
    {
        task.Title = updateDto.Title;
        task.Description = updateDto.Description;

        task.Priority = updateDto.Priority ?? task.Priority;
        
        task.Assignee = resolvedProperties.Assignee;
        task.AssigneeId = resolvedProperties.Assignee?.Id;

        task.Status = resolvedProperties.Status;
        task.TaskStatusId = resolvedProperties.Status?.Id;

        task.Release = resolvedProperties.Release;
        task.ReleaseId = resolvedProperties.Release?.Id;
    }
}