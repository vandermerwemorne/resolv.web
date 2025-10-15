namespace Resolv.Domain.Risk;

public class CustRiskLine
{
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public int? RiskId { get; set; }
    public DateTime InsertDate { get; set; }
    public DateTime? HazardDate { get; set; }
    public DateTime? AssignedDate { get; set; }
    public DateTime? CorrectiveActionDate { get; set; }
    public DateTime? Updated { get; set; }
    public string? DeptDivision { get; set; }
    public string? ReferenceNo { get; set; }
    public int? StepInOperationId { get; set; }
    public int? ClassificationId { get; set; }
    public string? Hazard { get; set; }
    public string? Risk { get; set; }
    public int? PictureId { get; set; }
    public int? SeverityId { get; set; }
    public int? FrequencyId { get; set; }
    public int? ExposureId { get; set; }
    public int? EngControlId { get; set; }
    public int? AdminControlId { get; set; }
    public int? PpeControlId { get; set; }
    public string? CurrentEngControls { get; set; }
    public string? RecEngControls { get; set; }
    public int? PercentageCompleteId { get; set; }
    public int? ManagementSuperId { get; set; }
    public int? ConformLegalReqId { get; set; }
    public string? CurrentAdminControls { get; set; }
    public string? CurrentManagementSuperControls { get; set; }
    public string? CurrentConformLegalReqControls { get; set; }
    public string? CurrentPpeControls { get; set; }
    public string? RecAdminControls { get; set; }
    public string? RecManagementSuperControls { get; set; }
    public string? RecConformLegalReqControls { get; set; }
    public string? RecPpeControls { get; set; }
    public string? AssignedToCompositeId { get; set; }
    public int? AddedByUserId { get; set; }
    public int RawRisk { get; set; } = 0;
    public int ResidualRisk { get; set; } = 0;
    public int EliminateId { get; set; } = 0;
    public string? EliminateRec { get; set; }
    public bool CanEdit { get; set; } = true;
    public int? UpdatedBy { get; set; }
    public int DataUpload { get; set; } = 0;
    public int AssignedToUserId { get; set; } = 0;
    public int CreatedUserId { get; set; } = 0;
    public int UpdatedUserId { get; set; } = 0;
    public int PrevRiskId { get; set; } = 0;
    public int NewResidualRisk { get; set; } = 0;
    public int StatusId { get; set; } = 1;
    public string? Observation { get; set; }
}
