using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Tasker.Domain;

namespace Tasker.Infrastructure.Data.Connection;

public class SqlConnectionFactory : IDatabaseConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_configuration.GetConnectionString("ApplicationConnection"));
    }
}