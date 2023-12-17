using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tasker.Domain.Repositories;

namespace Tasker.Infrastructure.Repositories;

public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected EntityRepository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
        => await DbSet.ToListAsync();

    public virtual async Task<TEntity?> GetByIdAsync(string id)
        => await DbSet.FindAsync(id);

    public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        => await DbSet.AsNoTracking().Where(predicate).ToListAsync();

    public async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        DbSet.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}