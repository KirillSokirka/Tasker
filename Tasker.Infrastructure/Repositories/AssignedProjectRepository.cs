using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Infrastructure.Repositories;

public class AssignedProjectRepository : EntityRepository<AssignedProjectUser>
{
    public AssignedProjectRepository(ApplicationContext context) : base(context)
    {
    }
}