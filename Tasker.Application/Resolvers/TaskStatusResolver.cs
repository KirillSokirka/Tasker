using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasker.Application.DTOs;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Infrastructure.Data.Application;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.Resolvers
{
    public class TaskStatusResolver : IResolver<TaskStatus, TaskStatusDto>
    {
        private readonly ApplicationContext _context;

        public TaskStatusResolver(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<TaskStatus> ResolveAsync(TaskStatusDto dto)
            => await _context.TaskStatuses.FirstOrDefaultAsync(p => p.Id == dto.Id)
               ?? throw new InvalidEntityException($"TaskStatus with id {dto.Id} doesnt exists");
    }
}
