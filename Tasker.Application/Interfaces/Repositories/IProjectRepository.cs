using Tasker.Application.DTOs;
using Tasker.Domain.Entities.Application;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        ProjectDTO? Get(string title);
        bool Create(ProjectDTO dto);
        bool Update(string id, ProjectDTO dto);
        bool Delete(string id);
    }
}
