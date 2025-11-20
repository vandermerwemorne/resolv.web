using Dapper;
using Resolv.Domain.AssessmentSite;

namespace Resolv.Infrastructure.AssessmentSite;

public class AssessmentSiteRepository(SqlConnectionFactory factory) : IAssessmentSiteRepository
{
    public async Task<(int, Guid)> AddAsync(CustAssessmentSite obj, string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var uid = Guid.NewGuid();
        var sql = $@"
INSERT INTO {schema}.assessment_site 
(added_by_user_id, site_name, ref_code, province_id, address, 
town_id, identity_code, division_id)
VALUES 
(@AddedByUserId, @SiteName, @RefCode, @ProvinceId, @Address, 
@TownId, @IdentityCode, @DivisionId)
RETURNING id, uid;";

        var result = await connection.QuerySingleAsync<(int, Guid)>(sql, new
        {
            obj.AddedByUserId,
            obj.SiteName,
            obj.RefCode,
            obj.ProvinceId,
            obj.Address,
            obj.TownId,
            obj.IdentityCode,
            obj.DivisionId
        });
        return result;
    }

    public async Task<List<CustAssessmentSite>> GetAsync(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.assessment_site
ORDER BY site_name;";

        var result = await connection.QueryAsync<CustAssessmentSite>(sql);
        return [.. result];
    }

    public async Task<List<CustAssessmentSite>> GetByDivisionIdAsync(string schema, int divisionId)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.assessment_site
WHERE division_id = @DivisionId
ORDER BY site_name;";

        var result = await connection.QueryAsync<CustAssessmentSite>(sql, new
        {
            DivisionId = divisionId
        });
        return [.. result];
    }

    public async Task<CustAssessmentSite> GetByUidAsync(string schema, Guid uid)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.assessment_site
WHERE uid = @Uid;";
        var result = await connection.QuerySingleOrDefaultAsync<CustAssessmentSite>(sql, new { Uid = uid });
        return result ?? new CustAssessmentSite { Id = 0 };
    }

    public async Task UpdateAsync(CustAssessmentSite obj, string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
UPDATE {schema}.assessment_site
SET site_name = @SiteName,
    ref_code = @RefCode,
    province_id = @ProvinceId,
    address = @Address,
    town_id = @TownId,
    identity_code = @IdentityCode,
    insert_date = @InsertDate
WHERE uid = @Uid;";
        await connection.ExecuteAsync(sql, new
        {
            obj.SiteName,
            obj.RefCode,
            obj.ProvinceId,
            obj.Address,
            obj.TownId,
            obj.IdentityCode,
            obj.InsertDate,
            obj.Uid
        });
    }
}
