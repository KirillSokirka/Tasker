using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.User;

namespace Tasker.Application.Interfaces.Services;

public interface IProjectService : IEntityService<ProjectDto>
{
    Task<ProjectDto> CreateAsync(ProjectCreateDto dto);
    Task<ProjectDto> UpdateAsync(ProjectUpdateDto dto);
    Task<List<MemberDto>> GetMembersAsync(string projectId);
}