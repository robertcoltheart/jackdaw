using Xunit;

namespace Jackdaw.Tests;

public class ClientConfigTests
{
    [Fact]
    public void CanSetSecurityProtocol()
    {
        var config = new ClientConfig
        {
            SecurityProtocol = SecurityProtocol.SaslPlaintext
        };

        var value = config.Get("security.protocol");

        Assert.Equal("sasl_plaintext", value);
        Assert.Equal(SecurityProtocol.SaslPlaintext, config.SecurityProtocol);
    }

    [Fact]
    public void CanSetEnumByNumber()
    {
        var config = new ClientConfig();
        config.Set("broker.address.family", "2");

        Assert.Equal(BrokerAddressFamily.V6, config.BrokerAddressFamily);
    }

    [Fact]
    public void CanSetEnumOvervalue()
    {
        var config = new ClientConfig
        {
            Acks = (Acks) 59
        };

        var value = config.Get("acks");

        Assert.Equal(59, (int) config.Acks!);
        Assert.Equal("59", value);
    }
}
