using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs;
using Tasker.Application.Exceptions;
using Tasker.Application.Interfaces.Repositories;
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

        public async Task<TaskStatusDto?> CreateAsync(TaskStatusDto statusDto)
        {
            if (await _context.TaskStatuses.AnyAsync(s => s.Name == statusDto.Name))
            {
                return null;
            }

            var status = _mapper.Map<TaskStatus>(statusDto);
            status.Id = Guid.NewGuid().ToString();

            await _context.TaskStatuses.AddAsync(status);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskStatusDto>(status);
        }

        public async Task<TaskStatusDto?> UpdateAsync(TaskStatusDto statusDto)
        {            
            var status = await _context.TaskStatuses.FindAsync(statusDto.Id);
            
            if (status is null)
            {
                return null;
            }

            if(! await _context.KanbanBoards.AnyAsync(b => b.Id == statusDto.KanbanBoardId))
            {
                throw new InvalidEntityException($"KanbanBoard with id {statusDto.KanbanBoardId} doesnt exists");
            }

            _mapper.Map(statusDto, status);

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

        public async Task<TaskStatusDto?> GetAsync(string id)
        {
            var status = await _context.TaskStatuses.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            
            return status is not null ? _mapper.Map<TaskStatusDto>(status) : null;
        }
    }

}