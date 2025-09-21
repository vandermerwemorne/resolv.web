using Resolv.Domain.Onboarding;
using Dapper;

namespace Resolv.Infrastructure.Onboarding;

/// <summary>
/// TODO - these functions should run as a separate task with a user that has elevanted `CREATE` permissions
/// </summary>
/// <param name="factory"></param>
public class CommonOnboardingRepository(SqlConnectionFactory factory) : ICommonOnboardingRepository
{
    public async Task AddCustomerSchema(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $"CREATE SCHEMA {schema};";
        await connection.ExecuteAsync(sql);
    }

    public async Task AddTableDivision(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql =
$@"CREATE TABLE {schema}.division (
id SERIAL PRIMARY KEY,
uid UUID DEFAULT gen_random_uuid(),
holding_company_id INTEGER,
name VARCHAR(45),
insert_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
added_by_user_id INTEGER);";
        await connection.ExecuteAsync(sql);
    }
}
