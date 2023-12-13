using Tasker.Application.DTOs.Application.Project;

namespace Tasker.Application.DTOs.Application.KanbanBoard;

public class KanbanBoardDto
{
    public string? Id { get; set; }
    public string Title { get; set; }
    public string ProjectId { get; set; }
    public ProjectDto? Project { get; set; }
    public List<TaskStatusDto> Columns { get; set; }
}