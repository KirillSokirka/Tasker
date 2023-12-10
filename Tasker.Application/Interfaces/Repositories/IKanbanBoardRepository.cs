using Tasker.Application.DTOs;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IKanbanBoardRepository
    {
        Task<KanbanBoardDto?> CreateAsync(KanbanBoardDto KanbanBoardDto);
        Task<KanbanBoardDto?> UpdateAsync(KanbanBoardDto KanbanBoardDto);
        Task<bool> DeleteAsync(string id);
        Task<KanbanBoardDto?> GetAsync(string id);
    }
}
