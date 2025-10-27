namespace Resolv.Domain.Classification;

public interface ISeverityRepository
{
    Task<List<ComSeverity>> GetComAsync();
}
