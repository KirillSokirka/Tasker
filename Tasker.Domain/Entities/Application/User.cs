namespace Tasker.Domain.Entities.Application;

public class User
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public List<Project>? AssignedProjects { get; set; }
    public List<Project>? UnderControlProjects { get; set; }
}