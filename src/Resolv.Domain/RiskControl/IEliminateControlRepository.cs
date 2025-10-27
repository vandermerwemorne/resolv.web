namespace Resolv.Domain.RiskControl;

public interface IEliminateControlRepository
{
    Task<List<EliminateControl>> GetAsync();
}
