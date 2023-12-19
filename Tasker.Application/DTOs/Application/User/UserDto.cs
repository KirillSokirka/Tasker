namespace Tasker.Application.DTOs.Application.User;

public class UserDto
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string? Title { get; set; }
    
    public List<string>? AssignedProjects { get; set; }
    public List<string>? UnderControlProjects { get; set; }
}