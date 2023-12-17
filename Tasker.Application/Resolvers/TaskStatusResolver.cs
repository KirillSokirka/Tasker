using Microsoft.EntityFrameworkCore;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Domain.Exceptions;
using Tasker.Infrastructure.Data.Application;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.Resolvers
{
    public class TaskStatusResolver : IResolver<TaskStatus, string>
    {
        private readonly ApplicationContext _context;

        public TaskStatusResolver(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<TaskStatus> ResolveAsync(string id)
            => await _context.TaskStatuses.FirstOrDefaultAsync(p => p.Id == id)
               ?? throw new InvalidEntityException($"TaskStatus with id {id} doesnt exists");
    }
}
