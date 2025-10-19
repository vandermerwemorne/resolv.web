namespace Resolv.Domain.Risk;

public interface IRiskLineRepository
{
    Task<List<CustRiskLine>> GetByRiskIdAsync(string schema, int riskId);
    Task<(int Id, Guid Uid)> AddEmptyAsync(string schema, int riskId);
    Task<(int Id, Guid Uid)> AddAsync(string schema, CustRiskLine riskline);
}
