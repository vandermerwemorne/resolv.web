namespace Resolv.Domain.RiskControl;

public interface ILegalRequirementControlRepository
{
    Task<List<LegalRequirementControl>> GetAsync();
}
