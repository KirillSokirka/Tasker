using Tasker.Application.DTOs.Application.KanbanBoard;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IKanbanBoardRepository
    {
        Task<KanbanBoardDto?> CreateAsync(KanbanBoardCreateDto dto);
        Task<KanbanBoardDto?> UpdateAsync(KanbanBoardDto dto);
        Task<bool> DeleteAsync(string id);
        Task<KanbanBoardDto?> GetAsync(string id);
        Task<List<KanbanBoardDto>> GetAllAsync();
    }
}
