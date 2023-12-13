using Tasker.Application.DTOs.Application.Project;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<ProjectDto?> CreateAsync(ProjectCreateDto projectDto);
        Task<ProjectDto?> UpdateAsync(ProjectUpdateDto projectDto);
        Task<bool> DeleteAsync(string id);
        Task<ProjectResultDto?> GetAsync(string id);
        Task<List<ProjectResultDto>> GetAllAsync();
    }
}
