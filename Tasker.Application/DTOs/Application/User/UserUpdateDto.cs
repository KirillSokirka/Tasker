namespace Tasker.Application.DTOs.Application.User;

public class UserUpdateDto
{
    public required string Id { get; set; }
    public string? Username { get; set; }
    public List<string>? AssignedProjects { get; set; }
    public List<string>? UnderControlProjects { get; set; }
}