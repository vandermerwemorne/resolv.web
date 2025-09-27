namespace Resolv.Domain.Onboarding;

public interface ICommonOnboardingRepository
{
    Task AddCustomerSchema(string schema);
    Task AddTableDivision(string schema);
    Task AddTableAssessmentSite(string schema);
    Task AddTableUser(string schema);
}
