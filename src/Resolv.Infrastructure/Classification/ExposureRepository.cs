using Dapper;
using Resolv.Domain.Classification;

namespace Resolv.Infrastructure.Classification;

public class ExposureRepository(SqlConnectionFactory factory) : IExposureRepository
{
    public async Task<List<ComExposure>> GetComAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.exposure
ORDER BY id;";

        var result = await connection.QueryAsync<ComExposure>(sql);
        return [.. result];
    }
}
