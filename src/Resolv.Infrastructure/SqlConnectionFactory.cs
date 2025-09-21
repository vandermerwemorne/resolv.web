using Npgsql;
using System.Data;

namespace Resolv.Infrastructure;

public class SqlConnectionFactory(string connectionString)
{
    public IDbConnection CreateNpgsqlConnection()
    {
        return new NpgsqlConnection(connectionString);
    }
}
