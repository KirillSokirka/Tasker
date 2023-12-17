using Tasker.Application.DTOs.Application.Release;

namespace Tasker.Application.Interfaces.Services;

public interface IReleaseService : IEntityService<ReleaseDto>
{
    Task<ReleaseDto> CreateAsync(ReleaseCreateDto dto);
    Task<ReleaseDto?> UpdateAsync(ReleaseUpdateDto dto);
}