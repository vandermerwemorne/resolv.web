namespace Resolv.Domain.Users;

public interface IComUserRepository
{
    /// <summary>
    /// These are our admin (software administrator) type users, not customers like Hulamin
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<ComUser> GetByCredentialsAsync(string username, string password);

    /// <summary>
    /// Get all users for a specific customer schema
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ComUser>> GetAsync();

    Task UpdateAsync(ComUser user);

    Task<ComUser> GetAsync(Guid uid);

    Task<(int Id, Guid Uid)> AddAsync(ComUser user);
}
