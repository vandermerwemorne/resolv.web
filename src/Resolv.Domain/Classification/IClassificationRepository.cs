namespace Resolv.Domain.Classification;

public interface IClassificationRepository
{
    Task<List<ComClassification>> GetComAsync();
}
