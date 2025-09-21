
using Resolv.Domain.Services;
using System.Security.Cryptography;
using System.Text;

namespace Resolv.Services;

public class EncryptionService : IEncryptionService
{
    public string Hash(string input, string salt)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var combined = input + salt;
        var bytes = Encoding.UTF8.GetBytes(combined);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexStringLower(hash);
    }
}
