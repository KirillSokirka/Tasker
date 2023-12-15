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
        => await DbSet
            .Include(p => p.KanbanBoards)
            .Include(p => p.Tasks)
            .Include(p => p.Releases)
            .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public override async Task<List<Project>> GetAllAsync() =>
        await DbSet
            .Include(p => p.KanbanBoards)
            .Include(p => p.Tasks)
            .Include(p => p.Releases)
            .AsNoTracking()
            .ToListAsync();
}