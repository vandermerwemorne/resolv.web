namespace Resolv.Domain.HazardCategory;

public interface IHazardCategoryRepository
{
    Task<List<ComHazardCategory>> GetComAsync();
    Task<List<CustHazardCategory>> GetCustAsync(string schema);
}
