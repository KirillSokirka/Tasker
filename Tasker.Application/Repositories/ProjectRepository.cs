using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public ProjectRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDto?> CreateAsync(ProjectDto projectDto)
        {
            if (await _context.Projects.AnyAsync(p => p.Title == projectDto.Title))
            {
                return null;
            }

            var project = _mapper.Map<Project>(projectDto);
            project.Id = Guid.NewGuid().ToString();

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto?> UpdateAsync(string id, ProjectDto projectDto)
        {
            projectDto.Id = id;
            
            var project = await _context.Projects.FindAsync(projectDto.Id);
            
            if (project is null)
            {
                return null;
            }

            _mapper.Map(projectDto, project);

            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var project = await _context.Projects.FindAsync(id);
            
            if (project is null)
            {
                return false;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ProjectDto?> GetAsync(string id)
        {
            var project = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            
            return project is not null ? _mapper.Map<ProjectDto>(project) : null;
        }
    }

}