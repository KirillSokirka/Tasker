using Tasker.Application.DTOs.Application.Project;

namespace Tasker.Application.DTOs.Application.User;

public class UserUpdateDto
{
    public required string Id { get; set; }
    public string? Username { get; set; }
    public List<UserProjectDto>? AssignedProjects { get; set; }
    public List<UserProjectDto>? UnderControlProjects { get; set; }
}
