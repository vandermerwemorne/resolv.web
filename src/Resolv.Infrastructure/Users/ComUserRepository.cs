using Resolv.Domain.Users;
using Dapper;

namespace Resolv.Infrastructure.Users;

public class ComUserRepository(SqlConnectionFactory factory) : IComUserRepository
{
    public async Task<ComUser> GetByCredentialsAsync(string username, string password)
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT id, uid, email, password, full_name, insert_date, has_access, added_by_user_id, roles, known_name
FROM common.user
WHERE email = @Username
AND password = @Password
AND has_access = TRUE";

        var user = await connection.QuerySingleOrDefaultAsync<ComUser>(sql, new { Username = username, Password = password });
        return user ?? new ComUser { Id = 0 };
    }

    public async Task<IEnumerable<ComUser>> GetAsync()
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.user
ORDER BY email DESC";

        var users = await connection.QueryAsync<ComUser>(sql);
        return users ?? [];
    }

    public async Task<ComUser> GetAsync(Guid uid)
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT *
FROM common.user
WHERE uid = @Uid";

        var user = await connection.QuerySingleOrDefaultAsync<ComUser>(sql, new { Uid = uid });
        return user ?? new ComUser { Id = 0 };
    }

    public async Task UpdateAsync(ComUser user)
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = $@"
UPDATE common.user 
SET email = @Email,
    password = @Password, 
    full_name = @FullName, 
    has_access = @HasAccess, 
    roles = @Roles, 
    known_name = @KnownName
WHERE uid = @Uid";

        user.InsertDate = DateTime.UtcNow;
        await connection.ExecuteAsync(sql, user);
    }

    public async Task<(int Id, Guid Uid)> AddAsync(ComUser user)
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
INSERT INTO common.user 
(uid, email, password, full_name, insert_date, has_access, added_by_user_id, roles, known_name)
VALUES 
(@Uid, @Email, @Password, @FullName, @InsertDate, @HasAccess, @AddedByUserId, @Roles, @KnownName)
RETURNING id, uid";

        user.Uid = Guid.NewGuid();
        user.InsertDate = DateTime.UtcNow;

        var result = await connection.QuerySingleAsync<(int Id, Guid Uid)>(sql, user);
        return result;
    }
}
