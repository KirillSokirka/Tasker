using Tasker.Application.DTOs;
using Tasker.Domain.Entities.Application;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<ProjectDto?> CreateAsync(ProjectDto projectDto);
        Task<ProjectDto?> UpdateAsync(string id, ProjectDto projectDto);
        Task<bool> DeleteAsync(string id);
        Task<ProjectDto?> GetAsync(string id);
    }
}
