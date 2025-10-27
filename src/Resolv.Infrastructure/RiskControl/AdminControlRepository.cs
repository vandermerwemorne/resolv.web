using Dapper;
using Resolv.Domain.RiskControl;

namespace Resolv.Infrastructure.RiskControl;

public class AdminControlRepository(SqlConnectionFactory factory) : IAdminControlRepository
{
    public async Task<List<AdminControl>> GetAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.admin_control
ORDER BY id;";

        var result = await connection.QueryAsync<AdminControl>(sql);
        return [.. result];
    }
}
