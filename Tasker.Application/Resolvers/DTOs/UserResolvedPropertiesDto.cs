using Tasker.Domain.Entities.Application;

namespace Tasker.Application.Resolvers.DTOs;

public class UserResolvedPropertiesDto
{
    public List<Project>? AssignedProjects { get; set; }
    public List<Project>? UnderControlProjects { get; set; }
}