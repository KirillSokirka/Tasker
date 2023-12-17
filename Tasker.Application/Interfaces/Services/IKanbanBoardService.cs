using Tasker.Application.DTOs.Application.KanbanBoard;

namespace Tasker.Application.Interfaces.Services;

public interface IKanbanBoardService : IEntityService<KanbanBoardDto>
{
    Task<KanbanBoardDto> CreateAsync(KanbanBoardCreateDto dto);
    Task<KanbanBoardDto> UpdateAsync(KanbanBoardUpdateDto dto);
}