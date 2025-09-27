namespace Resolv.Domain.AssessmentSite;

public interface IAssessmentSiteRepository
{
    Task<(int, Guid)> AddAsync(CustAssessmentSite obj, string schema);
    Task<List<CustAssessmentSite>> GetByDivisionIdAsync(string schema, int divisionId);
}
