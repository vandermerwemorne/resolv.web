using Dapper;
using Resolv.Domain.RiskControl;

namespace Resolv.Infrastructure.RiskControl;

public class LegalRequirementControlRepository(SqlConnectionFactory factory) : ILegalRequirementControlRepository
{
    public async Task<List<LegalRequirementControl>> GetAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.legal_requirement_control
ORDER BY id;";

        var result = await connection.QueryAsync<LegalRequirementControl>(sql);
        return [.. result];
    }
}
