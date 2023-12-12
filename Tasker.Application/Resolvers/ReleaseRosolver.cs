using Microsoft.EntityFrameworkCore;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Resolvers
{
    public class ReleaseResolver : IResolver<Release, string>
    {
        private readonly ApplicationContext _context;

        public ReleaseResolver(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Release> ResolveAsync(string id)
            => await _context.Releases.FirstOrDefaultAsync(p => p.Id == id)
               ?? throw new InvalidEntityException($"Release with id {id} doesnt exists");
    }
}
