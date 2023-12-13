using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.Release;
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

        public async Task<ReleaseDto?> CreateAsync(ReleaseCreateDto releaseDto)
        {
            if (await _context.Releases.AnyAsync(r => r.Title == releaseDto.Title))
            {
                return null;
            }

            var release = _mapper.Map<Release>(releaseDto);

            await _context.Releases.AddAsync(release);
            await _context.SaveChangesAsync();

            return _mapper.Map<ReleaseDto>(release);
        }

        public async Task<ReleaseDto?> UpdateAsync(ReleaseUpdateDto releaseDto)
        {
            var release = await _context.Releases.FindAsync(releaseDto.Id);
            
            if (release is null)
            {
                return null;
            }

            release.Title = releaseDto.Title ?? release.Title;
            release.IsReleased = releaseDto.IsReleased ?? release.IsReleased;
            release.EndDate = releaseDto.EndDate ?? release.EndDate;

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
            ReleaseDto? dto = null;
            if (release is not null)
            {
                var tasks = await _context.Tasks.Include(t => t.Status).AsNoTracking().Where(t => t.ReleaseId == release.Id).ToListAsync();


                dto = _mapper.Map<ReleaseDto>(release);
                dto.Tasks = tasks.Select(t => new ReleaseTaskDto() { Id = t.Id, Title = t.Title, TaskStatusName = t.Status.Name }).ToList();
            }

            return dto;
        }

        public async Task<List<ReleaseDto>> GetAllAsync() {
            var releases = await _context.Releases
            .AsNoTracking()
            .Select(release => _mapper.Map<ReleaseDto>(release))
            .ToListAsync();

            foreach(var release in releases)
            {
                    var tasks = await _context.Tasks.Include(t => t.Status).AsNoTracking().Where(t => t.ReleaseId == release.Id).ToListAsync();
                    release.Tasks = tasks.Select(t => new ReleaseTaskDto() { Id = t.Id, Title = t.Title, TaskStatusName = t.Status.Name }).ToList();
            }

                

            return releases;
        }
        
    }

}