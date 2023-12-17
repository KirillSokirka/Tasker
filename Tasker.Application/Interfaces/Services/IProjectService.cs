using Tasker.Application.DTOs.Application.Project;

namespace Tasker.Application.Interfaces.Services;

public interface IProjectService : IEntityService<ProjectDto>
{
    Task<ProjectDto> CreateAsync(ProjectCreateDto dto);
    Task<ProjectDto> UpdateAsync(ProjectUpdateDto dto);
}