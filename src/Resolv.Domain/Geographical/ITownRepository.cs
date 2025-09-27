namespace Resolv.Domain.Geographical;

public interface ITownRepository
{
    Task<List<Town>> GetAsync(int provinceId);
}
