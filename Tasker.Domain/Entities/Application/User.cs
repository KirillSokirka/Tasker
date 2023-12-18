namespace Tasker.Domain.Entities.Application;

public class User
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public List<AssignedProjectUser>? AssignedProjectUsers { get; set; }
    public List<AdminProjectUser>? AdminProjectUsers { get; set; }
}