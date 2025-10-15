namespace Resolv.Domain.Risk;

public interface IRiskRepository
{
    Task<CustRisk> GetAsync(string schema, Guid uid);
    Task<List<CustRisk>> GetByAssessmentSiteAsync(string schema, int assessmentSiteId);
}
