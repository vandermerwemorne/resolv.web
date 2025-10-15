namespace Resolv.Domain.Risk;

public interface IRiskLineRepository
{
    Task<List<CustRiskLine>> GetByRiskIdAsync(string schema, int riskId);
}
