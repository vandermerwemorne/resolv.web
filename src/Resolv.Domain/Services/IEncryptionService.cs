namespace Resolv.Domain.Services;

public interface IEncryptionService
{
    /// <summary>
    /// One-way encryption: Refers to hashing, which is irreversible and cannot be decrypted (e.g., SHA-256, bcrypt).
    /// </summary>
    /// <param name="input"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    string Hash(string input, string salt);
}
