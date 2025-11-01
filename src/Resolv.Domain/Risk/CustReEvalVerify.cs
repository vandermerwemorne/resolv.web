namespace Resolv.Domain.Risk;

public class CustReEvalVerify
{
    public int Id { get; set; }
    public DateTime InsertDate { get; set; }
    public int AddedByUserId { get; set; }
    public int ReEvalId { get; set; }
    public string? RecEngControls { get; set; }
    public string? RecAdminControls { get; set; }
    public string? RecSuperControls { get; set; }
    public string? RecPpeControls { get; set; }
    public string? RecLegalReqControls { get; set; }
    public int NewEngControlId { get; set; }
    public int NewAdminControlId { get; set; }
    public int NewManagementSuperId { get; set; }
    public int NewPpeControlId { get; set; }
    public int NewConformLegalReqId { get; set; }
    public int NewSeverityId { get; set; }
    public int NewFrequencyId { get; set; }
    public int NewExposureId { get; set; }
    public string? NewHazard { get; set; }
    public string? NewRisk { get; set; }
    public int NewResidualRisk { get; set; } = 0;
    public DateTime? UpdatedDate { get; set; }
    public int UpdatedBy { get; set; }
    public string? RecEliminateControls { get; set; }
    public int CreatedBy { get; set; } = 0;
}
