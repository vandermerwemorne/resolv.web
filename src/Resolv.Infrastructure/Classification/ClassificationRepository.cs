using Dapper;
using Resolv.Domain.Classification;

namespace Resolv.Infrastructure.Classification;

public class ClassificationRepository(SqlConnectionFactory factory) : IClassificationRepository
{
    public async Task<List<ComClassification>> GetComAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.master_classification
ORDER BY id;";

        var result = await connection.QueryAsync<ComClassification>(sql);
        return [.. result];
    }
}
