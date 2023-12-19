using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Resolvers;

public class ProjectResolver : IProjectResolver
{
    private readonly ApplicationContext _context;

    public ProjectResolver(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Project> ResolveAsync(string id)
        => await _context.Projects.FirstOrDefaultAsync(p => p.Id == id)
           ?? throw new InvalidEntityException($"Project with id {id} doesnt exists");
    
    public async Task<List<AdminProjectUser>> ResolveAdminProjectsAsync(
        Expression<Func<AdminProjectUser, bool>> predicate, List<UserProjectDto> userProjectDto)
    {
        var existingEntities = await _context.AdminProjectUsers.Where(predicate).ToListAsync();

        var newEntities = userProjectDto.Where(entity => existingEntities.Exists(exEntity =>
            exEntity.UserId == entity.UserId && exEntity.ProjectId == entity.UserId)).ToList();

        if (!newEntities.Any())
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

                _context.AdminProjectUsers.Add(newEntity);
                await _context.SaveChangesAsync();

                existingEntities.Add(newEntity);
            }
        }

        return existingEntities;
    }

    public async Task<List<AssignedProjectUser>> ResolveAssignedProjectsAsync(
        Expression<Func<AssignedProjectUser, bool>> predicate, List<UserProjectDto> userProjectDto)
    {
        var existingEntities = await _context.AssignedProjectUsers.Where(predicate).ToListAsync();

        var newEntities = userProjectDto.Where(entity => existingEntities.Exists(exEntity =>
            exEntity.UserId == entity.UserId && exEntity.ProjectId == entity.UserId)).ToList();

        if (!newEntities.Any())
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

                _context.AssignedProjectUsers.Add(newEntity);
                
                await _context.SaveChangesAsync();

                existingEntities.Add(newEntity);
            }
        }

        return existingEntities;
    }
    
    private async Task<User> ResolveUserAsync(string id)
        => await _context.User.FirstOrDefaultAsync(p => p.Id == id)
           ?? throw new InvalidEntityException($"Project with id {id} doesnt exists");
}