using Dapper;
using Resolv.Domain.Users;

namespace Resolv.Infrastructure.Users;

public class CustUserRepository(SqlConnectionFactory factory) : ICustUserRepository
{
    public async Task<IEnumerable<CustUser>> GetUsersAsync(string schemaName)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schemaName}.user
ORDER BY insert_date DESC";

        var users = await connection.QueryAsync<CustUser>(sql);
        return users ?? [];
    }

    public async Task<(int Id, Guid Uid)> AddUserAsync(CustUser user, string schemaName)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
INSERT INTO {schemaName}.user 
(uid, email, password, full_name, insert_date, has_access, added_by_user_id, roles, known_name)
VALUES 
(@Uid, @Email, @Password, @FullName, @InsertDate, @HasAccess, @AddedByUserId, @Roles, @KnownName)
RETURNING id, uid";

        user.Uid = Guid.NewGuid();
        user.InsertDate = DateTime.UtcNow;

        var result = await connection.QuerySingleAsync<(int Id, Guid Uid)>(sql, user);
        return result;
    }

    public async Task UpdateUserAsync(CustUser user, string schemaName)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
UPDATE {schemaName}.user 
SET email = @Email,
    password = @Password, 
    full_name = @FullName, 
    has_access = @HasAccess, 
    roles = @Roles, 
    known_name = @KnownName,
    insert_date = @InsertDate
WHERE uid = @Uid";

        user.InsertDate = DateTime.UtcNow;
        await connection.ExecuteAsync(sql, user);
    }

    public async Task<CustUser> GetUserAsync(string schemaName, Guid uid)
    {
        using var connection = factory.CreateNpgsqlConnection();
        var sql = $@"
SELECT *
FROM {schemaName}.user
WHERE uid = @Uid";

        var user = await connection.QuerySingleOrDefaultAsync<CustUser>(sql, new { Uid = uid });
        return user ?? new CustUser { Id = 0 };
    }
}
