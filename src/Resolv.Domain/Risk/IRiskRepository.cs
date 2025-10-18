namespace Resolv.Domain.Risk;

public interface IRiskRepository
{
    Task<CustRisk> GetAsync(string schema, Guid uid);
    Task<(int Id, Guid Uid)> AddAsync(CustRisk risk, string schema);
    Task<List<CustRisk>> GetByAssessmentSiteAsync(string schema, int assessmentSiteId);
}
