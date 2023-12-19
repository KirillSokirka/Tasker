using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Infrastructure.Repositories;

public class AdminProjectRepository : EntityRepository<AdminProjectUser>
{
    public AdminProjectRepository(ApplicationContext context) : base(context)
    {
    }
}