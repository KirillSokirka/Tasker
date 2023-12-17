using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.TaskStatus;

namespace Tasker.Application.DTOs.Application.KanbanBoard;

public class KanbanBoardDto
{
    public string? Id { get; set; }
    public string Title { get; set; } = null!;
    public string ProjectId { get; set; } = null!;
    public ProjectDto? Project { get; set; }
    public List<TaskStatusDto>? Columns { get; set; }
}