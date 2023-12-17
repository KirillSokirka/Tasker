namespace Tasker.Application.Interfaces;

public interface IEntityService<TDto> where TDto : class
{
    Task<TDto?> GetByIdAsync(string id);
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<bool> DeleteAsync(string id);
}