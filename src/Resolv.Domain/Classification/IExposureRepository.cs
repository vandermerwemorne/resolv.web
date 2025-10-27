namespace Resolv.Domain.Classification;

public interface IExposureRepository
{
    Task<List<ComExposure>> GetComAsync();
}
