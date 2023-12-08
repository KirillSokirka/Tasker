using Tasker.Application.Interfaces.Repositories;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationContext _context;

        public ProjectRepository(ApplicationContext context) { _context = context; }

        public bool Create(string title)
        {
            var proj = _context.Projects.FirstOrDefault(p => p.Title == title);

            if (proj != null)
            {
                return false;
            }

            _context.Projects.Add(new Project() { Id = Guid.NewGuid().ToString(), Title = title });
            _context.SaveChanges();

            return true;
        }

        public bool Update(string id, string title)
        {
            var proj = Get(id);

            if (proj == null)
            {
                return false;
            }

            proj.Title = title;
            _context.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var proj = Get(id);

            if (proj == null)
            {
                return false;
            }

            _context.Projects.Remove(proj);
            _context.SaveChanges();

            return true;
        }

        public Project? Get(string id)
        {
            return _context.Projects.FirstOrDefault(b => b.Id == id);
        }
    }
}
