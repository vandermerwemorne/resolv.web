using Dapper;
using Resolv.Domain.Division;

namespace Resolv.Infrastructure.Division;

public class CustDivisionRepository(SqlConnectionFactory factory) : ICustDivisionRepository
{
    public async Task<(int, Guid)> AddAsync(CustDivision obj, string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var uid = Guid.NewGuid();
        var sql = $@"
INSERT INTO {schema}.division (added_by_user_id, name)
VALUES (@AddedByUserId, @Name)
RETURNING id, uid;";

        var result = await connection.QuerySingleAsync<(int, Guid)>(sql, new
        {
            obj.AddedByUserId,
            obj.Name
        });
        return result;
    }

    public async Task<List<CustDivision>> GetAsync(string schema)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.division
ORDER BY name;";

        var result = await connection.QueryAsync<CustDivision>(sql);
        return [.. result];
    }

    public async Task<CustDivision> GetAsync(string schema, Guid uid)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schema}.division
WHERE uid = @Uid;";

        var result = await connection.QuerySingleOrDefaultAsync<CustDivision>(sql, new { Uid = uid });
        return result ?? new CustDivision { Id = 0 };
    }
}
