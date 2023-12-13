using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.Release;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.DTOs.Application.TaskStatus;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Domain.Exceptions;
using Tasker.Infrastructure.Data.Application;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.Repositories
{
    public class TaskStatusRepository : ITaskStatusRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public TaskStatusRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskStatusDto?> CreateAsync(TaskStatusCreateDto statusDto)
        {
            if (await _context.TaskStatuses.AnyAsync(s => s.Name == statusDto.Name))
            {
                return null;
            }

            var status = _mapper.Map<TaskStatus>(statusDto);

            await _context.TaskStatuses.AddAsync(status);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskStatusDto>(status);
        }

        public async Task<TaskStatusDto?> UpdateAsync(TaskStatusUpdateDto statusDto)
        {            
            var status = await _context.TaskStatuses.FindAsync(statusDto.Id);
            
            if (status is null)
            {
                return null;
            }

            if (statusDto.KanbanBoardId is not null)
            {
                await ValidateModel(statusDto);
            }

            status.Name = statusDto.Name ?? status.Name;
            status.KanbanBoardId = statusDto.KanbanBoardId ?? status.KanbanBoardId;

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskStatusDto>(status);
        }
        
        public async Task<bool> DeleteAsync(string id)
        {
            var status = await _context.TaskStatuses.FindAsync(id);
            
            if (status is null)
            {
                return false;
            }

            _context.TaskStatuses.Remove(status);
            await _context.SaveChangesAsync();

            return true;
        }
        
        private async Task ValidateModel(TaskStatusUpdateDto statusDto)
        {
            if (!await _context.KanbanBoards.AnyAsync(b => b.Id == statusDto.KanbanBoardId))
            {
                throw new InvalidEntityException($"KanbanBoard with id {statusDto.KanbanBoardId} doesnt exists");
            }
        }

        public async Task<TaskStatusDto?> GetAsync(string id)
        {
            var status = await _context.TaskStatuses.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

            TaskStatusDto? dto = null;
            if (status is not null)
            {
                var tasks = await _context.Tasks.Include(t => t.Status).AsNoTracking().Where(t => t.TaskStatusId == status.Id).ToListAsync();


                dto = _mapper.Map<TaskStatusDto>(status);
                dto.Tasks = tasks.Select(t => new PreviewTaskDto() { Id = t.Id, Title = t.Title!, TaskStatusName = t.Status?.Name ?? string.Empty }).ToList();
            }

            return dto;
        }

        public async Task<List<TaskStatusDto>> GetAllAsync() {
            var statuses = await _context.TaskStatuses
            .AsNoTracking()
            .Select(status => _mapper.Map<TaskStatusDto>(status))
            .ToListAsync();

            foreach (var status in statuses)
            {
                var tasks = await _context.Tasks.Include(t => t.Status).AsNoTracking().Where(t => t.TaskStatusId == status.Id).ToListAsync();
                status.Tasks = tasks.Select(t => new PreviewTaskDto() { Id = t.Id, Title = t.Title!, TaskStatusName = t.Status?.Name ?? string.Empty }).ToList();
            }
            return statuses;
        }
        
    }

}