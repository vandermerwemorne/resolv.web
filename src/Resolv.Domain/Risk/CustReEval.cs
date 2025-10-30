namespace Resolv.Domain.Risk;

public class CustReEval
{
    public int Id { get; set; }
    public int RiskId { get; set; }
    public int RiskLineId { get; set; }
    public DateTime InsertDate { get; set; }
    public string? CoractEngControls { get; set; }
    public string? CoractAdminControls { get; set; }
    public string? CoractSuperControls { get; set; }
    public string? CoractPpeControls { get; set; }
    public string? CoractLegalReqControls { get; set; }
    public int ReEvalStatusId { get; set; }
    public int AddedByUserId { get; set; }
    public int PictureId { get; set; }
    public string? CoractEliminate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int CreatedBy { get; set; }
}
