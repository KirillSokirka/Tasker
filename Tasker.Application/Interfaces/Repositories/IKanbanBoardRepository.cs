using Tasker.Application.DTOs.Application.KanbanBoard;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IKanbanBoardRepository
    {
        Task<KanbanBoardDto?> CreateAsync(KanbanBoardCreateDto dto);
        Task<KanbanBoardDto?> UpdateAsync(KanbanBoardUpdateDto dto);
        Task<bool> DeleteAsync(string id);
        Task<KanbanBoardResultDto?> GetAsync(string id);
        Task<List<KanbanBoardResultDto>> GetAllAsync();
    }
}
