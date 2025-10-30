namespace Resolv.Domain.Risk;

public interface ICustReEvalRepository
{
    /// <summary>
    /// Although a collection is possible there should only be one re-eval per risk line
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="riskLineId"></param>
    /// <returns></returns>
    Task<CustReEval> GetByRiskLineIdAsync(string schema, int riskLineId);
    Task<int> AddAsync(string schema, CustReEval reEval);
    Task UpdateAsync(string schema, CustReEval reEval);
}
