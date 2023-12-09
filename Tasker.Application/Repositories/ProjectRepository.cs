using AutoMapper;
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

        public bool Create(ProjectDTO dto)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Title == dto.Title);

            if (project != null)
            {
                return false;
            }

            _context.Projects.Add(new Project() { Id = Guid.NewGuid().ToString(), Title = dto.Title });
            _context.SaveChanges();

            return true;
        }

        public bool Update(string id, ProjectDTO dto)
        {
            var proj = _context.Projects.FirstOrDefault(b => b.Id == id);

            if (proj == null)
            {
                return false;
            }

            proj.Title = dto.Title;
            _context.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var proj = _context.Projects.FirstOrDefault(b => b.Id == id);

            if (proj == null)
            {
                return false;
            }

            _context.Projects.Remove(proj);
            _context.SaveChanges();

            return true;
        }

        public ProjectDTO? Get(string id)
        {
            var project = _context.Projects.FirstOrDefault(b => b.Id == id);
            return _mapper.Map<ProjectDTO>(project);
        }
    }
}
