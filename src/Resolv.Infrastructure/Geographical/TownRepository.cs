using Dapper;
using Resolv.Domain.Geographical;

namespace Resolv.Infrastructure.Geographical;

public class TownRepository(SqlConnectionFactory factory) : ITownRepository
{
    public async Task<List<Town>> GetAsync(int provinceId)
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.town
WHERE province_id = @provinceId
;";

        var result = await connection.QueryAsync<Town>(sql, new { provinceId });
        return [.. result];
    }
}
