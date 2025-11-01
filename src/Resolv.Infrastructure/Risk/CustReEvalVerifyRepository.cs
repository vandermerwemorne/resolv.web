using Dapper;
using Resolv.Domain.Risk;

namespace Resolv.Infrastructure.Risk;

public class CustReEvalVerifyRepository(SqlConnectionFactory factory) : ICustReEvalVerifyRepository
{
    public async Task<int> AddAsync(string schema, CustReEvalVerify reEvalVerify)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql =
            $@"
INSERT INTO 
{schema}.re_eval_verify(
insert_date, added_by_user_id, re_eval_id, rec_eng_controls, rec_admin_controls, rec_super_controls, rec_ppe_controls, rec_legal_req_controls, new_eng_control_id, new_admin_control_id, new_management_super_id, new_ppe_control_id, new_conform_legal_req_id, new_severity_id, new_frequency_id, new_exposure_id, new_hazard, new_risk, new_residual_risk, updated_date, updated_by, rec_eliminate_controls, created_by)
VALUES 
(@InsertDate, @AddedByUserId, @ReEvalId, @RecEngControls, @RecAdminControls, @RecSuperControls, @RecPpeControls, @RecLegalReqControls, @NewEngControlId, @NewAdminControlId, @NewManagementSuperId, @NewPpeControlId, @NewConformLegalReqId, @NewSeverityId, @NewFrequencyId, @NewExposureId, @NewHazard, @NewRisk, @NewResidualRisk, @UpdatedDate, @UpdatedBy, @RecEliminateControls, @CreatedBy)
RETURNING id";

        reEvalVerify.InsertDate = DateTime.UtcNow;

        var result = await connection.QuerySingleAsync<int>(sql, reEvalVerify);
        return result;
    }

    public async Task<CustReEvalVerify> GetByReEvalIdAsync(string schema, int reEvalId)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.re_eval_verify
WHERE re_eval_id = @ReEvalId
ORDER BY insert_date DESC
LIMIT 1;";

        var result = await connection.QuerySingleOrDefaultAsync<CustReEvalVerify>(sql, new { ReEvalId = reEvalId });
        return result ?? new() { Id = 0 };
    }

    public async Task UpdateAsync(string schema, CustReEvalVerify reEvalVerify)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
UPDATE {schema}.re_eval_verify
SET 
added_by_user_id=@AddedByUserId, 
re_eval_id=@ReEvalId, 
rec_eng_controls=@RecEngControls, 
rec_admin_controls=@RecAdminControls, 
rec_super_controls=@RecSuperControls, 
rec_ppe_controls=@RecPpeControls, 
rec_legal_req_controls=@RecLegalReqControls, 
new_eng_control_id=@NewEngControlId, 
new_admin_control_id=@NewAdminControlId, 
new_management_super_id=@NewManagementSuperId, 
new_ppe_control_id=@NewPpeControlId, 
new_conform_legal_req_id=@NewConformLegalReqId, 
new_severity_id=@NewSeverityId, 
new_frequency_id=@NewFrequencyId, 
new_exposure_id=@NewExposureId, 
new_hazard=@NewHazard, new_risk=@NewRisk, 
new_residual_risk=@NewResidualRisk, 
updated_date=@UpdatedDate, 
updated_by=@UpdatedBy, 
rec_eliminate_controls=@RecEliminateControls, 
created_by=@CreatedBy
WHERE id = @Id;";

        reEvalVerify.UpdatedDate = DateTime.UtcNow;
        await connection.ExecuteAsync(sql, reEvalVerify);
    }
}
