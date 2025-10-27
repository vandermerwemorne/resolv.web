namespace Resolv.Domain.RiskControl;

public interface IPPEControlRepository
{
    Task<List<PPEControl>> GetAsync();
}
