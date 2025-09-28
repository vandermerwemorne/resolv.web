namespace Resolv.Domain.AssessmentSite;

public interface IAssessmentSiteRepository
{
    Task<(int, Guid)> AddAsync(CustAssessmentSite obj, string schema);
    Task<List<CustAssessmentSite>> GetByDivisionIdAsync(string schema, int divisionId);
    Task UpdateAsync(CustAssessmentSite obj, string schema);
    Task<CustAssessmentSite> GetByUidAsync(string schema, Guid uid);
    Task<List<CustAssessmentSite>> GetAsync(string schema);
}
