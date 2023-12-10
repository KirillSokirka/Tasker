using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Repositories
{
    public class ReleaseRepository : IReleaseRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public ReleaseRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReleaseDto?> CreateAsync(ReleaseDto releaseDto)
        {
            if (await _context.Releases.AnyAsync(r => r.Title == releaseDto.Title))
            {
                return null;
            }

            var release = _mapper.Map<Release>(releaseDto);
            release.Id = Guid.NewGuid().ToString();
            release.CreationDate = DateTime.Now; // why is this here

            await _context.Releases.AddAsync(release);
            await _context.SaveChangesAsync();

            return _mapper.Map<ReleaseDto>(release);
        }

        public async Task<ReleaseDto?> UpdateAsync(ReleaseDto releaseDto)
        {
            var release = await _context.Releases.FindAsync(releaseDto.Id);
            
            if (release is null)
            {
                return null;
            }

            _mapper.Map(releaseDto, release);

            await _context.SaveChangesAsync();

            return _mapper.Map<ReleaseDto>(release);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var release = await _context.Releases.FindAsync(id);
            
            if (release is null)
            {
                return false;
            }

            _context.Releases.Remove(release);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ReleaseDto?> GetAsync(string id)
        {
            var release = await _context.Releases.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            
            return release is not null ? _mapper.Map<ReleaseDto>(release) : null;
        }
    }

}