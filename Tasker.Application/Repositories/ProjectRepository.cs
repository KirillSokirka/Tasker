using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;
using Task = Tasker.Domain.Entities.Application.Task;

namespace Tasker.Application.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IResolver<KanbanBoard, string> _boardResolver;
        private readonly IResolver<Task, string> _taskResolver;
        private readonly IResolver<Release, string> _releaseResolver;
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public ProjectRepository(ApplicationContext context, IMapper mapper,
            IResolver<KanbanBoard, string> boardResolver,
            IResolver<Task, string> taskResolver,
            IResolver<Release, string> releaseResolver)
        {
            _boardResolver = boardResolver;
            _taskResolver = taskResolver;
            _releaseResolver = releaseResolver;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDto?> CreateAsync(ProjectCreateDto projectDto)
        {
            if (await _context.Projects.AnyAsync(p => p.Title == projectDto.Title))
            {
                return null;
            }

            var project = new Project 
            { 
                Id = Guid.NewGuid().ToString(),
                Title = projectDto.Title,
                KanbanBoards = projectDto.KanbanBoardIds.Select(id => _boardResolver.ResolveAsync(id).Result).ToList(),
                Tasks = projectDto.TaskIds.Select(id => _taskResolver.ResolveAsync(id).Result).ToList(),
                Releases = projectDto.ReleaseIds.Select(id => _releaseResolver.ResolveAsync(id).Result).ToList()

            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto?> UpdateAsync(ProjectDto projectDto)
        {            
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

        public async Task<ProjectResultDto?> GetAsync(string id)
        {
            var project = await _context.Projects
                .Include(p => p.KanbanBoards)
                .Include(p => p.Tasks)
                .Include(p => p.Releases)
                .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            
            return project is not null ? _mapper.Map<ProjectResultDto>(project) : null;
        }

        public async Task<List<ProjectResultDto>> GetAllAsync() =>
        await _context.Projects
            .Include(p => p.KanbanBoards)
            .Include(p => p.Tasks)
            .Include(p => p.Releases)
            .AsNoTracking()
            .Select(project => _mapper.Map<ProjectResultDto>(project))
            .ToListAsync();
    }

}