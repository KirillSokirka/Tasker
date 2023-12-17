namespace Tasker.Application.DTOs.Application.ResolvedProperties;

public class UserResolvedPropertiesDto
{
    public List<Domain.Entities.Application.Project>? AssignedProjects { get; set; }
    public List<Domain.Entities.Application.Project>? UnderControlProjects { get; set; }
}