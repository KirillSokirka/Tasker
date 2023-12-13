namespace Tasker.Application.DTOs.Application.Project
{
    public class ProjectResultDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> KanbanBoardIds { get; set; }
        public List<string> TaskIds { get; set; }
        public List<string> ReleaseIds { get; set; }
    }
}
