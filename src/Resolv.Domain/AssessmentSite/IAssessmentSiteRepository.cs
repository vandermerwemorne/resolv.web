namespace Resolv.Domain.AssessmentSite;

public interface IAssessmentSiteRepository
{
    Task<List<CustAssessmentSite>> GetByDivisionIdAsync(string schema, int divisionId);
}
