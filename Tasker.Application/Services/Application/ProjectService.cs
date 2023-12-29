using AutoMapper;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.User;
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

        await Repository.AddAsync(project);

        await _project.ResolveAdminProjectsAsync(p => p.UserId == dto.UserId && p.ProjectId == project.Id,
                new List<UserProjectDto>
                {
                    new()
                    {
                        UserId = dto.UserId!,
                        ProjectId = project.Id
                    }
                });

        return (await GetByIdAsync(project.Id))!;
    }

    public async Task<ProjectDto> UpdateAsync(ProjectUpdateDto dto)
    {
        var project = (await Repository.FindAsync(project => project.Id == dto.Id)).FirstOrDefault() ??
                      throw new InvalidEntityException($"The project with id {dto.Id} is not found");

        project.Title = dto.Title ?? project.Title;

        await Repository.UpdateAsync(project);

        return (await GetByIdAsync(project.Id))!;
    }

    public async Task<List<MemberDto>> GetMembersAsync(string projectId)
    {
        var project = await Repository.GetByIdAsync(projectId) ??
                      throw new InvalidEntityException($"The project with id {projectId} is not found");

        return project.AdminProjectUsers?.Select(member => new MemberDto(member.UserId, member.User.Title, true))
            .Concat(project.AssignedProjectUsers?.Select(
                member => new MemberDto(member.UserId, member.User.Title, false)) ?? Array.Empty<MemberDto>())
            .DistinctBy(member => member.Id)
            .ToList() ?? new List<MemberDto>();
    }
}