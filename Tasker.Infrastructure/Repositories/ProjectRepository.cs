using Microsoft.EntityFrameworkCore;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Infrastructure.Repositories;

public class ProjectRepository : EntityRepository<Project>
{
    public ProjectRepository(ApplicationContext context) : base(context)
    {
    }

    public override async Task<Project?> GetByIdAsync(string id)
        =>
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            await DbSet
                .Include(p => p.KanbanBoards)
                .Include(p => p.Tasks).ThenInclude(p => p.Creator)
                .Include(p => p.Tasks).ThenInclude(p => p.Assignee)
                .Include(p => p.Releases)
                .Include(p => p.AdminProjectUsers).ThenInclude(p => p.User)
                .Include(p => p.AssignedProjectUsers).ThenInclude(p => p.User)
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
                .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    
    public override async Task<List<Project>> GetAllAsync() =>
        await DbSet
            .Include(p => p.KanbanBoards)
            .Include(p => p.Tasks).ThenInclude(p => p.Creator)
            .Include(p => p.Tasks).ThenInclude(p => p.Assignee)
            .Include(p => p.Releases)
            .Include(p => p.AdminProjectUsers)
            .Include(p => p.AssignedProjectUsers)
            .AsNoTracking()
            .ToListAsync();
}