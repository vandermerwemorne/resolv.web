using Resolv.Domain.Onboarding;
using Dapper;

namespace Resolv.Infrastructure.Onboarding;

/// <summary>
/// TODO - these functions should run as a separate task with a user that has elevanted `CREATE` permissions
/// </summary>
/// <param name="factory"></param>
public class CommonOnboardingRepository(SqlConnectionFactory factory) : ICommonOnboardingRepository
{
    private readonly string _owner = "resolv_web_admin";

    public async Task AddCustomerSchema(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $"CREATE SCHEMA {schema};";
        await connection.ExecuteAsync(sql);
    }

    public async Task AddTableAssessmentSite(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
CREATE TABLE IF NOT EXISTS {schema}.client 
(
id SERIAL PRIMARY KEY,
uid UUID DEFAULT gen_random_uuid(),
added_by_user_id INTEGER,
site_name VARCHAR(45),
ref_code VARCHAR(45),
insert_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
holding_company_id INTEGER,
province_id INTEGER,
address VARCHAR(500),
site_type_id INTEGER,
town_id INTEGER,
identity_code VARCHAR(45),
overdue_email BOOLEAN,
division_id INTEGER,
status BOOLEAN
);

ALTER TABLE IF EXISTS {schema}.client
OWNER to {_owner}";
        await connection.ExecuteAsync(sql);
    }

    public async Task AddTableDivision(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
CREATE TABLE {schema}.division 
(
id SERIAL PRIMARY KEY,
uid UUID DEFAULT gen_random_uuid(),
holding_company_id INTEGER,
name VARCHAR(45),
insert_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
added_by_user_id INTEGER
);

ALTER TABLE IF EXISTS {schema}.division
OWNER to {_owner}";
        await connection.ExecuteAsync(sql);
    }

    public async Task AddTableUser(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
CREATE TABLE {schema}.user 
(
id SERIAL PRIMARY KEY,
uid UUID DEFAULT gen_random_uuid(),
password VARCHAR(100),
full_name VARCHAR(45),
email VARCHAR(45),
insert_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
has_access BOOLEAN,
assessment_site_access VARCHAR(200),
added_by_user_id INTEGER,
roles VARCHAR(500),
known_name VARCHAR(45)
);

ALTER TABLE IF EXISTS {schema}.user
OWNER to {_owner}";
        await connection.ExecuteAsync(sql);
    }
}
