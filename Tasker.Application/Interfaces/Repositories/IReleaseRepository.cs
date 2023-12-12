using Tasker.Application.DTOs;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IReleaseRepository
    {
        Task<ReleaseDto?> CreateAsync(ReleaseDto projectDto);
        Task<ReleaseDto?> UpdateAsync(ReleaseDto projectDto);
        Task<bool> DeleteAsync(string id);
        Task<ReleaseDto?> GetAsync(string id);
        Task<List<ReleaseDto>> GetAllAsync();
    }
}
