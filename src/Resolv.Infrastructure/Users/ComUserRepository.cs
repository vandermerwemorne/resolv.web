using Resolv.Domain.Users;
using Dapper;

namespace Resolv.Infrastructure.Users;

public class ComUserRepository(SqlConnectionFactory factory) : IComUserRepository
{
    public async Task<ComUser> GetUserByCredentialsAsync(string username, string password)
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
}
