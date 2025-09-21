namespace Resolv.Domain.Users;

public interface ICommonUserRepository
{
    /// <summary>
    /// These are our admin (software administrator) type users, not customers like Hulamin
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<CommonUser> GetUserByCredentialsAsync(string username, string password);
}
