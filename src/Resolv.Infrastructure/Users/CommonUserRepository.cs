using Resolv.Domain.Users;
using Dapper;

namespace Resolv.Infrastructure.Users;

public class CommonUserRepository(SqlConnectionFactory factory) : ICommonUserRepository
{
    public async Task<CommonUser> GetUserByCredentialsAsync(string username, string password)
    {
        using var connection = factory.CreateNpgsqlConnection();
        const string sql = @"
SELECT id, uid, email, password, full_name, insert_date, has_access, added_by_user_id, roles, known_name
FROM common.user
WHERE email = @Username
AND password = @Password
AND has_access = TRUE";

        var user = await connection.QuerySingleOrDefaultAsync<CommonUser>(sql, new { Username = username, Password = password });
        return user ?? new CommonUser { Id = 0 };
    }
}
