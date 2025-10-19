namespace Resolv.Domain.Risk;

public interface IRiskLineRepository
{
    Task<List<CustRiskLine>> GetByRiskIdAsync(string schema, int riskId);
    Task<CustRiskLine?> GetByUidAsync(string schema, Guid uid);
    Task<(int Id, Guid Uid)> AddEmptyAsync(string schema, int riskId);
    Task<(int Id, Guid Uid)> AddAsync(string schema, CustRiskLine riskline);
    Task UpdateAsync(string schema, CustRiskLine riskLine);
}
