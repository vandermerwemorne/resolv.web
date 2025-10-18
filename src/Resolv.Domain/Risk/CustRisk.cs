namespace Resolv.Domain.Risk;

public class CustRisk
{
    public int Id { get; set; }
    public Guid Uid { get; set; }
    public DateTime InsertDate { get; set; }
    public DateTime? ReevaluationDate { get; set; }
    public int RiskStatusId { get; set; }
    public int EvaluationTypeId { get; set; }
    public int? RiskIdReEvaluation { get; set; }
    public int ClientId { get; set; }
    public int UserId { get; set; }
    public int SectorId { get; set; }
    public int SubSectorId { get; set; }
    public int AddedByUserId { get; set; }
    public int AnnualStatus { get; set; }
}
