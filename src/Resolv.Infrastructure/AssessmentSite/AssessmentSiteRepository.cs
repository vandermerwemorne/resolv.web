using Dapper;
using Resolv.Domain.AssessmentSite;

namespace Resolv.Infrastructure.AssessmentSite;

// TODO we need to rename dbo.client to be dbo.assessment_site

public class AssessmentSiteRepository(SqlConnectionFactory factory) : IAssessmentSiteRepository
{
    public async Task<(int, Guid)> AddAsync(CustAssessmentSite obj, string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var uid = Guid.NewGuid();
        var sql = $@"
INSERT INTO {schema}.client 
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

    public async Task<List<CustAssessmentSite>> GetByDivisionIdAsync(string schema, int divisionId)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.client
WHERE division_id = @DivisionId
ORDER BY site_name;";

        var result = await connection.QueryAsync<CustAssessmentSite>(sql, new
        {
            DivisionId = divisionId
        });
        return [.. result];
    }
}
