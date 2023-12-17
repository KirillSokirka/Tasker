using Microsoft.EntityFrameworkCore;
using Tasker.Infrastructure.Data.Application;
using Task = Tasker.Domain.Entities.Application.Task;

namespace Tasker.Infrastructure.Repositories;

public class TaskRepository : EntityRepository<Task>
{
    public TaskRepository(ApplicationContext context) : base(context)
    {
    }
    
    public override async Task<Task?> GetByIdAsync(string id)
        => await DbSet
            .Include(t => t.Project)
            .Include(t => t.Assignee)
            .Include(t => t.Release)
            .Include(t => t.Creator)
            .Include(t => t.Status)
            .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public override async Task<List<Task>> GetAllAsync() =>
        await DbSet
            .Include(t => t.Project)
            .Include(t => t.Assignee)
            .Include(t => t.Release)
            .Include(t => t.Creator)
            .Include(t => t.Status)
            .AsNoTracking()
            .ToListAsync();
}