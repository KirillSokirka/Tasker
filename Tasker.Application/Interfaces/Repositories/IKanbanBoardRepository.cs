using Tasker.Application.DTOs;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IKanbanBoardRepository
    {
        Task<KanbanBoardDto?> CreateAsync(KanbanBoardDto dto);
        Task<KanbanBoardDto?> UpdateAsync(KanbanBoardDto dto);
        Task<bool> DeleteAsync(string id);
        Task<KanbanBoardDto?> GetAsync(string id);
    }
}
