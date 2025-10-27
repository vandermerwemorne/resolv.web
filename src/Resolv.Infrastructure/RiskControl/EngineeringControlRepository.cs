using Dapper;
using Resolv.Domain.RiskControl;

namespace Resolv.Infrastructure.RiskControl;

public class EngineeringControlRepository(SqlConnectionFactory factory) : IEngineeringControlRepository
{
    public async Task<List<EngineeringControl>> GetAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.engineering_control
ORDER BY id;";

        var result = await connection.QueryAsync<EngineeringControl>(sql);
        return [.. result];
    }
}
