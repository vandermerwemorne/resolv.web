namespace Resolv.Domain.Onboarding;

public interface ICommonOnboardingRepository
{
    Task AddCustomerSchema(string schema);

    Task AddTableDivision(string schema);
    Task AddTableAssessmentSite(string schema);
    Task AddTableUser(string schema);
    Task AddTableRisk(string schema);
    Task AddTableRiskLine(string schema);
    Task AddTableRiskImages(string schema);

    /// <summary>
    /// TODO `step_in_operation` need to change to be `hazard_category`
    /// </summary>
    /// <param name="schema"></param>
    /// <returns></returns>
    Task AddTableHazardCategory(string schema);

    /// <summary>
    /// TODO `master_step_in_operation` need to change to be `master_hazard_category`
    /// </summary>
    /// <param name="schema"></param>
    /// <returns></returns>
    Task CopyHazardCategory(string schema);
}
