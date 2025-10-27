using Dapper;
using Resolv.Domain.Classification;

namespace Resolv.Infrastructure.Classification;

public class SeverityRepository(SqlConnectionFactory factory) : ISeverityRepository
{
    public async Task<List<ComSeverity>> GetComAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.severity
ORDER BY id;";

        var result = await connection.QueryAsync<ComSeverity>(sql);
        return [.. result];
    }
}
