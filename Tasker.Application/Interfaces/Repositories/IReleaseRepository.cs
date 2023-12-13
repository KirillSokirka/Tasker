using Tasker.Application.DTOs.Application.Release;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IReleaseRepository
    {
        Task<ReleaseDto?> CreateAsync(ReleaseCreateDto projectDto);
        Task<ReleaseDto?> UpdateAsync(ReleaseUpdateDto projectDto);
        Task<bool> DeleteAsync(string id);
        Task<ReleaseDto?> GetAsync(string id);
        Task<List<ReleaseDto>> GetAllAsync();
    }
}
