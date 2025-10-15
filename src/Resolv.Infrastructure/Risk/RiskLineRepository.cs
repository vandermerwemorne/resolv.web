using Dapper;
using Resolv.Domain.Risk;

namespace Resolv.Infrastructure.Risk;

public class RiskLineRepository(SqlConnectionFactory factory) : IRiskLineRepository
{
    public async Task<List<CustRiskLine>> GetByRiskIdAsync(string schema, int riskId)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.risk_line
WHERE risk_id = @riskId
ORDER BY insert_date ASC;";

        var result = await connection.QueryAsync<CustRiskLine>(sql, new { riskId });
        return [.. result];
    }
}
