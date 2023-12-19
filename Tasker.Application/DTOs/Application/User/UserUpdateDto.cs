using Tasker.Application.DTOs.Application.Project;

namespace Tasker.Application.DTOs.Application.User;

public class UserUpdateDto
{
    public string? Id { get; set; }
    public required string Username { get; set; }
    public List<UserProjectDto>? AssignedProjects { get; set; }
    public List<UserProjectDto>? UnderControlProjects { get; set; }
}
