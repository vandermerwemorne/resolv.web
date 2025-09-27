namespace Resolv.Domain.Geographical;

public interface IProvinceRepository
{
    Task<List<Province>> GetAsync();
}
