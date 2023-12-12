namespace Tasker.Application.DTOs.Application.KanbanBoard
{
    public class KanbanBoardCreateDto
    {
        public string Title { get; set; }
        public string ProjectId { get; set; }
        public List<string> ColumnIds { get; set; }
    }
}
