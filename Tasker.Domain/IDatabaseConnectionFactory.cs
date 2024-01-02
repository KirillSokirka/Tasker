using System.Data;

namespace Tasker.Domain;

public interface IDatabaseConnectionFactory
{
    IDbConnection CreateConnection();
}