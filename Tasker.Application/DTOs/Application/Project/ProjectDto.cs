using Tasker.Application.DTOs.Application.KanbanBoard;
using Tasker.Application.DTOs.Application.Release;
using Tasker.Application.DTOs.Application.Task;

namespace Tasker.Application.DTOs.Application.Project;

public class ProjectDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public List<KanbanBoardDto> KanbanBoards { get; set; }
    public List<TaskDto> Tasks { get; set; }
    public List<ReleaseDto> Releases { get; set; } 
    public List<string>? AssignedUsers { get; set; }
    public List<string>? AdminProjects { get; set; }
}