using Dapper;
using Resolv.Domain.HoldingCompany;

namespace Resolv.Infrastructure.HoldingCompany;

public class HoldingCompanyRepository(SqlConnectionFactory factory) : IHoldingCompanyRepository
{
    public async Task<(int, Guid)> AddAsync(ComHoldingCompany obj)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var uid = Guid.NewGuid();
        const string sql = @"
INSERT INTO common.holding_company (added_by_user_id, name)
VALUES (@AddedByUserId, @Name)
RETURNING id, uid;";

        var result = await connection.QuerySingleAsync<(int, Guid)>(sql, new
        {
            obj.AddedByUserId,
            obj.Name
        });
        return result;
    }
    public async Task<ComHoldingCompany> GetAsync(Guid uid)
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.holding_company
WHERE uid = @Uid;";

        var result = await connection.QuerySingleOrDefaultAsync<ComHoldingCompany>(sql, new { Uid = uid });
        return result ?? new ComHoldingCompany { Id = 0 };
    }
    public async Task<List<ComHoldingCompany>> GetAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
            SELECT *
            FROM common.holding_company;";

        var result = await connection.QueryAsync<ComHoldingCompany>(sql);
        return [.. result];
    }
    public async Task<ComHoldingCompany> GetAsync(string name)
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.holding_company
WHERE name = @name;";

        var result = await connection.QuerySingleOrDefaultAsync<ComHoldingCompany>(sql, new { name });
        return result ?? new ComHoldingCompany { Id = 0 };
    }
}
