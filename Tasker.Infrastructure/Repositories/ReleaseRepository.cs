using Microsoft.EntityFrameworkCore;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Infrastructure.Repositories;

public class ReleaseRepository : EntityRepository<Release>
{
    public ReleaseRepository(ApplicationContext context) : base(context)
    {
    }

    public override async Task<Release?> GetByIdAsync(string id)
        => await DbSet
            .Include(r => r.Tasks).ThenInclude( t => t.Status)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

    public override async Task<List<Release>> GetAllAsync()
        => await DbSet
            .Include(r => r.Tasks)
            .AsNoTracking()
            .ToListAsync();
}