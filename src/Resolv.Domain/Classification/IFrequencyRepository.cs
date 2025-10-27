namespace Resolv.Domain.Classification;

public interface IFrequencyRepository
{
    Task<List<ComFrequency>> GetComAsync();
}
