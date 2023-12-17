using AutoMapper;
using Tasker.Application.DTOs.Application.Release;
using Tasker.Application.Interfaces.Services;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Application.Services.Application;

public class ReleaseService : EntityService<Release, ReleaseDto>, IReleaseService
{
    public ReleaseService(IEntityRepository<Release> repository, IMapper mapper) : base(repository, mapper)
    {
    }

    public async Task<ReleaseDto> CreateAsync(ReleaseCreateDto dto)
    {
        await ValidateRelease(dto.Title, dto.ProjectId);

        var release = Mapper.Map<Release>(dto);

        await Repository.AddAsync(release);

        return (await GetByIdAsync(release.Id))!;
    }
    
    public async Task<ReleaseDto?> UpdateAsync(ReleaseUpdateDto dto)
    {
        var release = await Repository.GetByIdAsync(dto.Id) ??
                      throw new InvalidEntityException($"The release with id {dto.Id} is not found");

        if (dto.Title is not null)
        {
            await ValidateRelease(dto.Title, dto.ProjectId);
            
            release.Title = dto.Title ?? release.Title;
        }
        
        release.IsReleased = dto.IsReleased ?? release.IsReleased;
        release.EndDate = dto.EndDate ?? release.EndDate;
        
        await Repository.UpdateAsync(release);
        
        return (await GetByIdAsync(release.Id))!;
    }
    
    private async Task ValidateRelease(string title, string projectId)
    {
        var result =
            (await Repository.FindAsync(r => r.Title == title && r.ProjectId == projectId))
            .FirstOrDefault() is not null;

        if (!result)
        {
            throw new InvalidEntityException($"The same release is already exist in current project");
        }
    }
}