namespace Resolv.Domain.RiskControl;

public interface IAdminControlRepository
{
    Task<List<AdminControl>> GetAsync();
}
