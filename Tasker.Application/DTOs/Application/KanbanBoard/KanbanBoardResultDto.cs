namespace Tasker.Application.DTOs.Application.KanbanBoard
{
    public class KanbanBoardResultDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ProjectId { get; set; }
        public List<string> ColumnIds { get; set; }
    }
}
