using Dapper;
using Resolv.Domain.RiskControl;

namespace Resolv.Infrastructure.RiskControl;

public class EliminateControlRepository(SqlConnectionFactory factory) : IEliminateControlRepository
{
    public async Task<List<EliminateControl>> GetAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.eliminate_control
ORDER BY id;";

        var result = await connection.QueryAsync<EliminateControl>(sql);
        return [.. result];
    }
}
