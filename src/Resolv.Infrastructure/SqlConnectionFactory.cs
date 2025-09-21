using Npgsql;
using System.Data;

namespace Resolv.Infrastructure;

public class SqlConnectionFactory(string connectionString)
{
    public IDbConnection CreateNpgsqlConnection()
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        return new NpgsqlConnection(connectionString);
    }
}
