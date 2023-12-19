using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Infrastructure.Repositories;

public class UserRepository : EntityRepository<User>
{
    public UserRepository(ApplicationContext context) : base(context)
    { }

    public override async Task<User?> GetByIdAsync(string id)
        => await DbSet.AsNoTracking()
            .Include(u => u.AssignedProjectUsers)
            .Include(u => u.AdminProjectUsers)
            .FirstOrDefaultAsync(user => user.Id == id);

    public override async Task<List<User>> GetAllAsync()
        => await DbSet.AsNoTracking()
            .Include(u => u.AssignedProjectUsers)
            .Include(u => u.AdminProjectUsers)
            .ToListAsync();

    public override async Task<List<User>> FindAsync(Expression<Func<User, bool>> predicate)
        => await DbSet.AsNoTracking()
        .Include(u => u.AssignedProjectUsers)
        .Include(u => u.AdminProjectUsers)
        .Where(predicate).ToListAsync();
}
