namespace Resolv.Domain.Users;

public interface ICustUserRepository
{
    /// <summary>
    /// Get all users for a specific customer schema
    /// </summary>
    /// <param name="schemaName">The customer schema name</param>
    /// <returns></returns>
    Task<IEnumerable<CustUser>> GetUsersAsync(string schemaName);

    /// <summary>
    /// Add a new user to a specific customer schema
    /// </summary>
    /// <param name="user">The user to add</param>
    /// <param name="schemaName">The customer schema name</param>
    /// <returns></returns>
    Task<(int Id, Guid Uid)> AddUserAsync(CustUser user, string schemaName);

    /// <summary>
    /// Update an existing user in a specific customer schema
    /// </summary>
    /// <param name="user">The user to update</param>
    /// <param name="schemaName">The customer schema name</param>
    /// <returns></returns>
    Task UpdateUserAsync(CustUser user, string schemaName);

    /// <summary>
    /// Get a user by UID from a specific customer schema
    /// </summary>
    /// <param name="schemaName">The customer schema name</param>
    /// <param name="uid">The user UID</param>
    /// <returns></returns>
    Task<CustUser> GetUserAsync(string schemaName, Guid uid);

    /// <summary>
    /// These are customer users, eg Hulamin
    /// </summary>
    /// <param name="schemaName"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<CustUser> GetByCredentialsAsync(string schemaName, string username, string password);
}
