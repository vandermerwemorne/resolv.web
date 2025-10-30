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

    public async Task AddTableRisk(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
CREATE TABLE {schema}.risk 
(
id SERIAL PRIMARY KEY,
insert_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
reevaluation_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
risk_status_id INTEGER,
evaluation_type_id INTEGER,
risk_id_re_evaluation INTEGER,
client_id INTEGER,
user_id INTEGER,
sector_id INTEGER,
sub_sector_id INTEGER,
added_by_user_id INTEGER,
annual_status INTEGER,
uid UUID DEFAULT gen_random_uuid()
);

ALTER TABLE IF EXISTS {schema}.risk
OWNER to {_owner}";
        await connection.ExecuteAsync(sql);
    }

    public async Task AddTableRiskLine(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
CREATE TABLE {schema}.risk_line 
(
id SERIAL PRIMARY KEY,
uid UUID DEFAULT gen_random_uuid(),
risk_id INTEGER,
insert_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
dept_division VARCHAR(45),
reference_no VARCHAR(125),
hazard_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
step_in_operation_id INTEGER,
classification_id INTEGER,
hazard VARCHAR(1000),
risk VARCHAR(1000),
picture_id INTEGER,
severity_id INTEGER,
frequency_id INTEGER,
exposure_id INTEGER,
eng_control_id INTEGER,
admin_control_id INTEGER,
ppe_control_id INTEGER,
current_eng_controls VARCHAR(1000),
rec_eng_controls VARCHAR(1000),
assigned_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
corrective_action_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
percentage_complete_id INTEGER,
management_super_id INTEGER,
conform_legal_req_id INTEGER,
current_admin_controls VARCHAR(1000),
current_management_super_controls VARCHAR(1000),
current_conform_legal_req_controls VARCHAR(1000),
current_ppe_controls VARCHAR(1000),
rec_admin_controls VARCHAR(1000),
rec_management_super_controls VARCHAR(1000),
rec_conform_legal_req_controls VARCHAR(1000),
rec_ppe_controls VARCHAR(1000),
assigned_to_composite_id VARCHAR(50),
added_by_user_id INTEGER,
raw_risk INTEGER,
residual_risk INTEGER,
eliminate_id INTEGER,
eliminate_rec VARCHAR(1000),
updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
can_edit BOOLEAN,
updated_by INTEGER,
data_upload INTEGER,
assigned_to_user_id INTEGER,
created_user_id INTEGER,
updated_user_id INTEGER,
prev_risk_id INTEGER,
new_residual_risk INTEGER,
status_id INTEGER,
observation VARCHAR(500)
);

ALTER TABLE IF EXISTS {schema}.risk_line
OWNER to {_owner}";
        await connection.ExecuteAsync(sql);
    }

    public async Task AddTableRiskImages(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
CREATE TABLE {schema}.risk_images 
(
id SERIAL PRIMARY KEY,
risk_line_id INTEGER,
image_name VARCHAR(200),
image_blob bytea,
insert_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
height INTEGER,
width INTEGER,
user_agent VARCHAR(1000)
);

ALTER TABLE IF EXISTS {schema}.risk_images
OWNER to {_owner}";
        await connection.ExecuteAsync(sql);
    }

    public async Task AddTableHazardCategory(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
CREATE TABLE {schema}.step_in_operation 
(
id SERIAL PRIMARY KEY,
description VARCHAR(200),
insert_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
inc_in_calc BOOLEAN DEFAULT true,
enabled BOOLEAN DEFAULT true
);

ALTER TABLE IF EXISTS {schema}.step_in_operation
OWNER to {_owner}";
        await connection.ExecuteAsync(sql);
    }

    public async Task CopyHazardCategory(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var data = await connection.QueryAsync<string>("SELECT description FROM common.master_step_in_operation ORDER BY description;");
        foreach (var item in data)
        {
            var sql = $@"
INSERT INTO {schema}.step_in_operation (description)
VALUES ('{item}');
";
            await connection.ExecuteAsync(sql);
        }
    }

    public async Task AddTableReEval(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
CREATE TABLE {schema}.re_eval 
(
id SERIAL PRIMARY KEY,
risk_id INTEGER,
risk_line_id INTEGER,
insert_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
coract_eng_controls VARCHAR(500),
coract_admin_controls VARCHAR(500),
coract_super_controls VARCHAR(500),
coract_ppe_controls VARCHAR(500),
coract_legal_req_controls VARCHAR(500),
re_eval_status_id INTEGER,
added_by_user_id INTEGER,
picture_id INTEGER,
coract_eliminate VARCHAR(500),
updated_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
created_by INTEGER);

ALTER TABLE IF EXISTS {schema}.re_eval
OWNER to {_owner}";
        await connection.ExecuteAsync(sql);
    }
}
