namespace Resolv.Domain.Users;

public interface IComUserRepository
{
    /// <summary>
    /// These are our admin (software administrator) type users, not customers like Hulamin
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<ComUser> GetUserByCredentialsAsync(string username, string password);
}
