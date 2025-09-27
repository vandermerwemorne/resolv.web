using Dapper;
using Resolv.Domain.Geographical;

namespace Resolv.Infrastructure.Geographical;

public class ProvinceRepository(SqlConnectionFactory factory) : IProvinceRepository
{
    public async Task<List<Province>> GetAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.province
ORDER BY name;"
        ;

        var result = await connection.QueryAsync<Province>(sql);
        return [.. result];
    }
}
