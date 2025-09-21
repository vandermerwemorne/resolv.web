namespace Resolv.Domain.Division;

public interface ICustDivisionRepository
{
    Task<(int, Guid)> AddAsync(CustDivision obj, string schema);
    Task<List<CustDivision>> GetAsync(string schema);
}
