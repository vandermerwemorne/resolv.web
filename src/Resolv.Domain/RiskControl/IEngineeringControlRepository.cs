namespace Resolv.Domain.RiskControl;

public interface IEngineeringControlRepository
{
    Task<List<EngineeringControl>> GetAsync();
}
