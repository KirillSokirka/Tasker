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

namespace Tasker.Application.Resolvers
{
    public class ReleaseResolver : IResolver<Release, ReleaseDto>
    {
        private readonly ApplicationContext _context;

        public ReleaseResolver(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Release> ResolveAsync(ReleaseDto dto)
            => await _context.Releases.FirstOrDefaultAsync(p => p.Id == dto.Id)
               ?? throw new InvalidEntityException($"Release with id {dto.Id} doesnt exists");
    }
}
