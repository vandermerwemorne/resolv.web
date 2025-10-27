using Dapper;
using Resolv.Domain.Classification;

namespace Resolv.Infrastructure.Classification;

public class FrequencyRepository(SqlConnectionFactory factory) : IFrequencyRepository
{
    public async Task<List<ComFrequency>> GetComAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.frequency
ORDER BY id;";

        var result = await connection.QueryAsync<ComFrequency>(sql);
        return [.. result];
    }
}
