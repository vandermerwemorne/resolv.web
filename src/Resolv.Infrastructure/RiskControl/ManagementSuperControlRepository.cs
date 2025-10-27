using Dapper;
using Resolv.Domain.RiskControl;

namespace Resolv.Infrastructure.RiskControl;

public class ManagementSuperControlRepository(SqlConnectionFactory factory) : IManagementSuperControlRepository
{
    public async Task<List<ManagementSuperControl>> GetAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.management_super_control
ORDER BY id;";

        var result = await connection.QueryAsync<ManagementSuperControl>(sql);
        return [.. result];
    }
}
