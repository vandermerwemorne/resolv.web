using Dapper;
using Resolv.Domain.AssessmentSite;

namespace Resolv.Infrastructure.AssessmentSite;

// TODO we need to rename dbo.client to be dbo.assessment_site

public class AssessmentSiteRepository(SqlConnectionFactory factory) : IAssessmentSiteRepository
{
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
