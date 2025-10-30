namespace Resolv.Domain.Risk;

public interface IReEvalStatusRepository
{
    Task<List<ReEvalStatus>> GetAsync();
}
