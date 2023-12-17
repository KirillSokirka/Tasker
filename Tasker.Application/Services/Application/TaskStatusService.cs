using AutoMapper;
using Tasker.Application.DTOs.Application.TaskStatus;
using Tasker.Application.Interfaces.Services;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Domain.Repositories;
using Task = System.Threading.Tasks.Task;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.Services.Application;

public class TaskStatusService : EntityService<TaskStatus, TaskStatusDto>, ITaskStatusService
{
    private readonly IEntityRepository<KanbanBoard> _kanbanBoardRepository;

    public TaskStatusService(IEntityRepository<TaskStatus> repository, IMapper mapper,
        IEntityRepository<KanbanBoard> kanbanBoardRepository) : base(repository, mapper)
    {
        _kanbanBoardRepository = kanbanBoardRepository;
    }

    public async Task<TaskStatusDto> CreateAsync(TaskStatusCreateDto dto)
    {
        await ValidateTaskStatusAsync(dto.KanbanBoardId, dto.Name);

        var status = Mapper.Map<TaskStatus>(dto);

        await Repository.AddAsync(status);

        return (await GetByIdAsync(status.Id))!;
    }

    public async Task<TaskStatusDto> UpdateAsync(TaskStatusUpdateDto dto)
    {
        var status = await Repository.GetByIdAsync(dto.Id) ?? 
                     throw new InvalidEntityException($"The status with id = {dto.Id} doesn't exits");

        await ValidateTaskStatusAsync(dto.KanbanBoardId, dto.Name ?? null);
        
        status.Name = dto.Name ?? status.Name;

        await Repository.UpdateAsync(status);
        
        return (await GetByIdAsync(status.Id))!;
    }

    private async Task ValidateTaskStatusAsync(string boardId, string? statusName)
    {
        var board = await _kanbanBoardRepository.GetByIdAsync(boardId)
                    ?? throw new InvalidEntityException($"The kanban board with id = {boardId} doesn't exits");

        if (statusName is not null && board.Columns.Any(c => c.Name == statusName))
        {
            throw new InvalidEntityException(
                $"The column with name = {board.Title} already has column with name = {statusName}");
        }
    }
}