using Tasker.Domain.Entities.Application;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Project? Get(string title);
        bool Create(string title);
        bool Update(string id, string title);
        bool Delete(string id);
    }
}
