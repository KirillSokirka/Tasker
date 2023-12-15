using AutoMapper;
using Tasker.Domain.Repositories;

namespace Tasker.Application.Services.Application;

public class EntityService<TEntity, TDto> 
    where TEntity : class
    where TDto : class
{
    protected readonly IEntityRepository<TEntity> Repository;
    protected readonly IMapper Mapper;

    protected EntityService(IEntityRepository<TEntity> repository, IMapper mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }

    public async Task<TDto?> GetByIdAsync(string id)
    {
        var entity = await Repository.GetByIdAsync(id);
        
        return entity is null ? null : Mapper.Map<TDto>(entity);
    }

    public async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await Repository.GetAllAsync();
        
        return Mapper.Map<IEnumerable<TDto>>(entities);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await Repository.GetByIdAsync(id);
        
        if (entity is not null)
        {
            await Repository.DeleteAsync(entity);
            
            return true;
        }

        return false;
    }
}