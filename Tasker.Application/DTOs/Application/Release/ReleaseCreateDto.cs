namespace Tasker.Application.DTOs.Application.Release
{
    public class ReleaseCreateDto
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public required string Title { get; set; }
        public required string ProjectId { get; set; }
        public bool IsReleased { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
    }
}
