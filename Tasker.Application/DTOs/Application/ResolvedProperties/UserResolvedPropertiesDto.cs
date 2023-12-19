using Tasker.Domain.Entities.Application;

namespace Tasker.Application.DTOs.Application.ResolvedProperties;

public class UserResolvedPropertiesDto
{
    public List<AssignedProjectUser>? AssignedProjects { get; set; }
    public List<AdminProjectUser>? UnderControlProjects { get; set; }
}