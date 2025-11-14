using Dapper;
using Resolv.Domain.Risk;

namespace Resolv.Infrastructure.Risk;

public class CustReEvalRepository(SqlConnectionFactory factory) : ICustReEvalRepository
{
    public async Task<int> AddAsync(string schema, CustReEval reEval)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql =
            $@"
INSERT INTO 
{schema}.re_eval(
risk_id, risk_line_id, insert_date, coract_eng_controls, coract_admin_controls, coract_super_controls, coract_ppe_controls, coract_legal_req_controls, re_eval_status_id, added_by_user_id, picture_id, coract_eliminate, updated_date, created_by)
VALUES 
(@RiskId, @RiskLineId, @InsertDate, @CoractEngControls, @CoractAdminControls, @CoractSuperControls, @CoractPpeControls, @CoractLegalReqControls, @ReEvalStatusId, @AddedByUserId, @PictureId, @CoractEliminate, @UpdatedDate, @CreatedBy)
RETURNING id";

        reEval.InsertDate = DateTime.UtcNow;

        var result = await connection.QuerySingleAsync<int>(sql, reEval);
        return result;
    }

    public async Task<List<CustReEval>> GetByRiskIdsAsync(string schema, List<int> riskIds)
    {
        if (riskIds == null || riskIds.Count == 0)
            return [];

        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.re_eval
WHERE risk_id = ANY(@RiskIds)
ORDER BY insert_date DESC";

        var result = await connection.QueryAsync<CustReEval>(sql, new { RiskIds = riskIds.ToArray() });
        return [.. result];
    }

    public async Task<CustReEval> GetByRiskLineIdAsync(string schema, int riskLineId)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.re_eval
WHERE risk_line_id = @RiskLineId
ORDER BY insert_date DESC
LIMIT 1;";

        var result = await connection.QuerySingleOrDefaultAsync<CustReEval>(sql, new { RiskLineId = riskLineId });
        return result ?? new() { Id = 0 };
    }

    public async Task<CustReEval> GetByUidAsync(string schema, Guid uid)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.re_eval
WHERE uid = @uid";

        var result = await connection.QuerySingleOrDefaultAsync<CustReEval>(sql, new { uid });
        return result ?? new() { Id = 0 };
    }

    public async Task UpdateAsync(string schema, CustReEval reEval)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
UPDATE {schema}.re_eval
SET 
coract_eng_controls=@CoractEngControls, 
coract_admin_controls=@CoractAdminControls, 
coract_super_controls=@CoractSuperControls, 
coract_ppe_controls=@CoractPpeControls, 
coract_legal_req_controls=@CoractLegalReqControls, 
re_eval_status_id=@ReEvalStatusId, 
picture_id=@PictureId, 
coract_eliminate=@CoractEliminate, 
updated_date=@UpdatedDate
WHERE id = @Id;";

        reEval.UpdatedDate = DateTime.UtcNow;
        await connection.ExecuteAsync(sql, reEval);
    }
}
