namespace Tasker.Domain.Entities.Application;

public class AdminProjectUser
{
    public string ProjectId { get; set; } = null!;
    public Project Project { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}