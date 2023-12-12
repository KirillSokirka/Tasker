namespace Tasker.Application.DTOs.Application;

public class KanbanBoardDto : IdentityDto
{
    public string Title { get; set; }
    public string? ProjectId { get; set; }
}