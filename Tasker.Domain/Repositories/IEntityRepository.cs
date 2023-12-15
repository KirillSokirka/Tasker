namespace Tasker.Domain.Repositories;

public interface IEntityRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(string id);
    Task<List<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}