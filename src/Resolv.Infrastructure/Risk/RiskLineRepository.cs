using Dapper;
using Resolv.Domain.Risk;

namespace Resolv.Infrastructure.Risk;

public class RiskLineRepository(SqlConnectionFactory factory) : IRiskLineRepository
{
    public async Task<(int Id, Guid Uid)> AddAsync(string schema, CustRiskLine riskline)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql =
            $@"
INSERT INTO 
{schema}.risk_line(
risk_id, insert_date, dept_division, reference_no, hazard_date, step_in_operation_id, classification_id, hazard, risk, picture_id, severity_id, frequency_id, exposure_id, eng_control_id, admin_control_id, ppe_control_id, current_eng_controls, rec_eng_controls, assigned_date, corrective_action_date, percentage_complete_id, management_super_id, conform_legal_req_id, current_admin_controls, current_management_super_controls, current_conform_legal_req_controls, current_ppe_controls, rec_admin_controls, rec_management_super_controls, rec_conform_legal_req_controls, rec_ppe_controls, assigned_to_composite_id, added_by_user_id, raw_risk, residual_risk, eliminate_id, eliminate_rec, updated, can_edit, updated_by, data_upload, assigned_to_user_id, created_user_id, updated_user_id, prev_risk_id, new_residual_risk, status_id, observation)
VALUES 
(@RiskId, @InsertDate, @DeptDivision, @ReferenceNo, @HazardDate, @StepInOperationId, @ClassificationId, @Hazard, @Risk, @PictureId, @SeverityId, @FrequencyId, @ExposureId, @EngControlId, @AdminControlId, @PpeControlId, @CurrentEngControls, @RecEngControls, @AssignedDate, @CorrectiveActionDate, @PercentageCompleteId, @ManagementSuperId, @ConformLegalReqId, @CurrentAdminControls, @CurrentManagementSuperControls, @CurrentConformLegalReqControls, @CurrentPpeControls, @RecAdminControls, @RecManagementSuperControls, @RecConformLegalReqControls, @RecPpeControls, @AssignedToCompositeId, @AddedByUserId, @RawRisk, @ResidualRisk, @EliminateId, @EliminateRec, @Updated, @CanEdit, @UpdatedBy, @DataUpload, @AssignedToUserId, @CreatedUserId, @UpdatedUserId, @PrevRiskId, @NewResidualRisk, @StatusId, @Observation)
RETURNING id, uid";

        riskline.InsertDate = DateTime.UtcNow;

        var result = await connection.QuerySingleAsync<(int Id, Guid Uid)>(sql, riskline);
        return result;
    }

    public async Task<(int Id, Guid Uid)> AddEmptyAsync(string schema, int riskId)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql =
            $@"
INSERT INTO 
{schema}.risk_line(
risk_id)
VALUES 
(@RiskId)
RETURNING id, uid";

        var result = await connection.QuerySingleAsync<(int Id, Guid Uid)>(sql, new
        {
            RiskId = riskId
        });
        return result;
    }

    public async Task<CustRiskLine?> GetByIdAsync(string schema, int riskLineId)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.risk_line
WHERE id = @riskLineId;";

        var result = await connection.QuerySingleOrDefaultAsync<CustRiskLine>(sql, new { riskLineId });
        return result;
    }

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

    public async Task<CustRiskLine?> GetByUidAsync(string schema, Guid uid)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.risk_line
WHERE uid = @uid;";

        var result = await connection.QuerySingleOrDefaultAsync<CustRiskLine>(sql, new { uid });
        return result;
    }

    public async Task UpdateAsync(string schema, CustRiskLine riskLine)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
UPDATE {schema}.risk_line
SET 
    dept_division = @DeptDivision,
    reference_no = @ReferenceNo,
    step_in_operation_id = @StepInOperationId,
    classification_id = @ClassificationId,
    hazard = @Hazard,
    risk = @Risk,
    severity_id = @SeverityId,
    frequency_id = @FrequencyId,
    exposure_id = @ExposureId,
    eliminate_id = @EliminateId,
    eliminate_rec = @EliminateRec,
    eng_control_id = @EngControlId,
    current_eng_controls = @CurrentEngControls,
    rec_eng_controls = @RecEngControls,
    admin_control_id = @AdminControlId,
    current_admin_controls = @CurrentAdminControls,
    rec_admin_controls = @RecAdminControls,
    management_super_id = @ManagementSuperId,
    current_management_super_controls = @CurrentManagementSuperControls,
    rec_management_super_controls = @RecManagementSuperControls,
    ppe_control_id = @PpeControlId,
    current_ppe_controls = @CurrentPpeControls,
    rec_ppe_controls = @RecPpeControls,
    conform_legal_req_id = @ConformLegalReqId,
    current_conform_legal_req_controls = @CurrentConformLegalReqControls,
    rec_conform_legal_req_controls = @RecConformLegalReqControls,
    assigned_to_composite_id = @AssignedToCompositeId,
    assigned_date = @AssignedDate,
    corrective_action_date = @CorrectiveActionDate,
    updated = @Updated,
    updated_by = @UpdatedBy
WHERE uid = @Uid;";

        riskLine.Updated = DateTime.UtcNow;
        await connection.ExecuteAsync(sql, riskLine);
    }
}
