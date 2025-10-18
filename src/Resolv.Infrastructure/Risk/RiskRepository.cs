using Dapper;
using Resolv.Domain.Risk;

namespace Resolv.Infrastructure.Risk;

public class RiskRepository(SqlConnectionFactory factory) : IRiskRepository
{
    public async Task<(int Id, Guid Uid)> AddAsync(CustRisk risk, string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
INSERT INTO {schema}.risk 
(insert_date, reevaluation_date, risk_status_id, evaluation_type_id, risk_id_re_evaluation, client_id, user_id, sector_id, sub_sector_id, added_by_user_id, annual_status, uid)
VALUES
(@InsertDate, @ReevaluationDate, @RiskStatusId, @EvaluationTypeId, @RiskIdReEvaluation, @ClientId, @UserId, @SectorId, @SubSectorId, @AddedByUserId, @AnnualStatus, @Uid)
RETURNING id, uid";

        risk.Uid = Guid.NewGuid();
        risk.InsertDate = DateTime.UtcNow;

        var result = await connection.QuerySingleAsync<(int Id, Guid Uid)>(sql, risk);
        return result;
    }

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
