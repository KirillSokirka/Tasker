using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Infrastructure.Repositories;

public class UserRepository : EntityRepository<User>
{
    public UserRepository(ApplicationContext context) : base(context)
    { }
}