using Dapper;
using Resolv.Domain.Risk;

namespace Resolv.Infrastructure.Risk;

public class RiskRepository(SqlConnectionFactory factory) : IRiskRepository
{
    public async Task<CustRisk> GetAsync(string schema, Guid uid)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.risk
WHERE uid = @Uid;";

        var result = await connection.QuerySingleOrDefaultAsync<CustRisk>(sql, new { Uid = uid });
        return result ?? new CustRisk { Id = 0 };
    }

    public async Task<List<CustRisk>> GetByAssessmentSiteAsync(string schema, int assessmentSiteId)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.risk
WHERE client_id = @assessmentSiteId
ORDER BY insert_date ASC;";

        var result = await connection.QueryAsync<CustRisk>(sql, new { assessmentSiteId });
        return [.. result];
    }
}
