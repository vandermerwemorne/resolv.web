using Dapper;
using Resolv.Domain.HazardCategory;

namespace Resolv.Infrastructure.HazardCategory;

/// <summary>
/// The db tables master_step_in_operation, step_in_operation will be renamed to hazard_category in future. 
/// </summary>
/// <param name="factory"></param>
public class HazardCategoryRepository(SqlConnectionFactory factory) : IHazardCategoryRepository
{
    public async Task<List<ComHazardCategory>> GetComAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.master_step_in_operation
ORDER BY description;";

        var result = await connection.QueryAsync<ComHazardCategory>(sql);
        return [.. result];
    }

    public async Task<List<CustHazardCategory>> GetCustAsync(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.step_in_operation
ORDER BY description;";

        var result = await connection.QueryAsync<CustHazardCategory>(sql);
        return [.. result];
    }
}
