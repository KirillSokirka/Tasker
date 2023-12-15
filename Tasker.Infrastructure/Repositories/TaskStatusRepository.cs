using Microsoft.EntityFrameworkCore;
using Tasker.Infrastructure.Data.Application;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Infrastructure.Repositories;

public class TaskStatusRepository : EntityRepository<TaskStatus>
{
    public TaskStatusRepository(ApplicationContext context) : base(context)
    {
    }

    public override async Task<TaskStatus?> GetByIdAsync(string id)
        => await DbSet
            .Include(t => t.Tasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

    public override async Task<List<TaskStatus>> GetAllAsync()
        => await DbSet
            .Include(t => t.Tasks)
            .AsNoTracking()
            .ToListAsync();
}