using Microsoft.EntityFrameworkCore;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Exceptions;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Resolvers
{
    public class TaskResolver : IResolver<Domain.Entities.Application.Task, string>
    {
        private readonly ApplicationContext _context;

        public TaskResolver(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Application.Task> ResolveAsync(string id) => await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id)
               ?? throw new InvalidEntityException($"Task with id {id} doesnt exists");
    }
}
