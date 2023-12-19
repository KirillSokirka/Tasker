using AutoMapper;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Application.Interfaces.Services;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Domain.Repositories;

namespace Tasker.Application.Services.Application;

public class ProjectService : EntityService<Project, ProjectDto>, IProjectService
{
    private readonly IProjectResolver _project;

    public ProjectService(IEntityRepository<Project> repository, IMapper mapper, IProjectResolver project) : base(
        repository, mapper)
    {
        _project = project;
    }

    public async Task<ProjectDto> CreateAsync(ProjectCreateDto dto)
    {
        var project = new Project
        {
            Title = dto.Title
        };

        project.AdminProjectUsers =
            await _project.ResolveAdminProjectsAsync(p => p.UserId == dto.UserId && p.ProjectId == project.Id,
                new List<UserProjectDto>
                {
                    new()
                    {
                        UserId = dto.UserId!,
                        ProjectId = project.Id
                    }
                });

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