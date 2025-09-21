namespace Resolv.Domain.HoldingCompany;

public interface IHoldingCompanyRepository
{
    Task<(int, Guid)> AddAsync(ComHoldingCompany obj);
    Task<ComHoldingCompany> GetAsync(Guid uid);
    Task<List<ComHoldingCompany>> GetAsync();
    Task<ComHoldingCompany> GetAsync(string name);
}
