using Dapper;
using Resolv.Domain.Risk;

namespace Resolv.Infrastructure.Risk;

public class ReEvalStatusRepository(SqlConnectionFactory factory) : IReEvalStatusRepository
{
    public async Task<List<ReEvalStatus>> GetAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.re_eval_status
ORDER BY id;";

        var result = await connection.QueryAsync<ReEvalStatus>(sql);
        return [.. result];
    }
}
