using Tasker.Application.DTOs;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<ProjectDto?> CreateAsync(ProjectDto projectDto);
        Task<ProjectDto?> UpdateAsync(ProjectDto projectDto);
        Task<bool> DeleteAsync(string id);
        Task<ProjectDto?> GetAsync(string id);
        Task<List<ProjectDto>> GetAllAsync();
    }
}
