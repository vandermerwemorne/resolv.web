using Dapper;
using Resolv.Domain.RiskControl;

namespace Resolv.Infrastructure.RiskControl;

public class PPEControlRepository(SqlConnectionFactory factory) : IPPEControlRepository
{
    public async Task<List<PPEControl>> GetAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.ppe_control
ORDER BY id;";

        var result = await connection.QueryAsync<PPEControl>(sql);
        return [.. result];
    }
}
