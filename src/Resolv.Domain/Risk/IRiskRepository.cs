namespace Resolv.Domain.Risk;

public interface IRiskRepository
{
    Task<List<CustRisk>> GetByAssessmentSiteAsync(string schema, int assessmentSiteId);
}
