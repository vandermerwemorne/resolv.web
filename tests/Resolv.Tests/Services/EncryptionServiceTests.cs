
using Resolv.Services;

namespace Resolv.Tests.Services;

public class EncryptionServiceTests
{
    [Fact]
    public void Hash_ReturnsExpectedHash_ForKnownInput()
    {
        // Arrange
        var service = new EncryptionService();
        var input = "Password1";
        var salt = "carl.paton@gmail.com";
        var expectedHash = "98760f7567ea6a153249c8b830483926feb1e9e59b5d9385b6dde98e8e138f20";

        // Act
        var result = service.Hash(input, salt);

        // Assert
        Assert.Equal(expectedHash, result);
    }

    [Fact]
    public void Hash_ReturnsEmptyString_ForNullOrEmptyInput()
    {
        var service = new EncryptionService();
        var salt = "anysalt";
        Assert.Equal(string.Empty, service.Hash(string.Empty, salt));
    }
}
