namespace Resolv.Domain.RiskControl;

public interface IManagementSuperControlRepository
{
    Task<List<ManagementSuperControl>> GetAsync();
}
