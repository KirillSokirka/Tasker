using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Domain.Repositories;
using Tasker.Infrastructure.Data.Application;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Application.Resolvers;

public class ProjectResolver : IProjectResolver
{
    private readonly ApplicationContext _context;
    private readonly IEntityRepository<AdminProjectUser> _adminRepository;
    private readonly IEntityRepository<AssignedProjectUser> _assignedRepository;

    public ProjectResolver(ApplicationContext context, IEntityRepository<AdminProjectUser> adminRepository,
        IEntityRepository<AssignedProjectUser> assignedRepository)
    {
        _context = context;
        _adminRepository = adminRepository;
        _assignedRepository = assignedRepository;
    }

    public async Task<Project> ResolveAsync(string id)
        => await _context.Projects.FirstOrDefaultAsync(p => p.Id == id)
           ?? throw new InvalidEntityException($"Project with id {id} doesnt exists");

    public async Task ResolveAdminProjectsAsync(
        Expression<Func<AdminProjectUser, bool>> predicate, List<UserProjectDto>? userProjectDto)
    {
        var existingEntities = await _adminRepository.FindAsync(predicate);

        if (existingEntities.Any())
        {
            foreach (var entity in existingEntities)
            {
                await _adminRepository.DeleteAsync(entity);
            }
        }

        var newEntities = userProjectDto.Where(entity
            => !existingEntities.Exists(exEntity =>
                exEntity.UserId == entity.UserId && exEntity.ProjectId == entity.ProjectId)).ToList();

        if (newEntities.Any())
        {
            foreach (var entity in newEntities)
            {
                var newEntity = new AdminProjectUser
                {
                    User = await ResolveUserAsync(entity.UserId),
                    Project = await ResolveAsync(entity.ProjectId),
                    UserId = entity.UserId,
                    ProjectId = entity.ProjectId
                };

                await _adminRepository.AddAsync(newEntity);
            }
        }
    }

    public async Task ResolveAssignedProjectsAsync(
        Expression<Func<AssignedProjectUser, bool>> predicate, List<UserProjectDto>? userProjectDto)
    {
        var existingEntities = await _assignedRepository.FindAsync(predicate);

        if (existingEntities.Any())
        {
            foreach (var entity in existingEntities)
            {
                await _assignedRepository.DeleteAsync(entity);
            }
        }

        var newEntities = userProjectDto.Where(entity
            => !existingEntities.Exists(exEntity =>
                exEntity.UserId == entity.UserId && exEntity.ProjectId == entity.ProjectId)).ToList();

        if (newEntities.Any())
        {
            foreach (var entity in newEntities)
            {
                var newEntity = new AssignedProjectUser
                {
                    User = await ResolveUserAsync(entity.UserId),
                    Project = await ResolveAsync(entity.ProjectId),
                    UserId = entity.UserId,
                    ProjectId = entity.ProjectId
                };

                await _assignedRepository.AddAsync(newEntity);
            }
        }
    }

    private async Task<User> ResolveUserAsync(string id)
        => await _context.User.FirstOrDefaultAsync(p => p.Id == id)
           ?? throw new InvalidEntityException($"Project with id {id} doesnt exists");
}