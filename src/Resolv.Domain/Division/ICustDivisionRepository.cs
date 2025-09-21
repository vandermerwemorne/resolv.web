namespace Resolv.Domain.Division;

public interface ICustDivisionRepository
{
    Task<(int, Guid)> AddAsync(CustDivision obj, string schema);
}
