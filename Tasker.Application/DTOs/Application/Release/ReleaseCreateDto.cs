namespace Tasker.Application.DTOs.Application.Release
{
    public class ReleaseCreateDto
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public bool IsReleased { get; set; } = false;

        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }

        public string ProjectId { get; set; } = null!;
    }
}
