namespace Resolv.Domain.Risk;

public interface ICustReEvalVerifyRepository
{
    Task<CustReEvalVerify> GetByReEvalIdAsync(string schema, int reEvalId);
    Task<int> AddAsync(string schema, CustReEvalVerify reEvalVerify);
    Task UpdateAsync(string schema, CustReEvalVerify reEvalVerify);
}
