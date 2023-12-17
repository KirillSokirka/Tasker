using AutoMapper;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.Interfaces.Services;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Domain.Repositories;

namespace Tasker.Application.Services.Application;

public class ProjectService : EntityService<Project, ProjectDto>, IProjectService
{
    public ProjectService(IEntityRepository<Project> repository, IMapper mapper) : base(repository, mapper)
    { }

    public async Task<ProjectDto> CreateAsync(ProjectCreateDto dto)
    {
        var project = new Project
        {
            Title = dto.Title
        };

        await Repository.AddAsync(project);
        
        return (await GetByIdAsync(project.Id))!;
    }

    public async Task<ProjectDto> UpdateAsync(ProjectUpdateDto dto)
    {
        var project = await Repository.GetByIdAsync(dto.Id) ??
                      throw new InvalidEntityException($"The project with id {dto.Id} is not found");

        project.Title = dto.Title ?? project.Title;
        
        await Repository.UpdateAsync(project);
        
        return (await GetByIdAsync(project.Id))!;
    }
}