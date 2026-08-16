namespace Jackdaw.Tests;

public class ClientConfigTests
{
    [Test]
    public async Task CanSetSecurityProtocol()
    {
        var config = new ClientConfig
        {
            SecurityProtocol = SecurityProtocol.SaslPlaintext
        };

        var value = config.Get("security.protocol");

        await Assert.That(value).IsEqualTo("sasl_plaintext");
        await Assert.That(config.SecurityProtocol).IsEqualTo(SecurityProtocol.SaslPlaintext);
    }

    [Test]
    public async Task CanSetEnumByNumber()
    {
        var config = new ClientConfig();
        config.Set("broker.address.family", "2");

        await Assert.That(config.BrokerAddressFamily).IsEqualTo(BrokerAddressFamily.V6);
    }

    [Test]
    public async Task CanSetEnumOvervalue()
    {
        var config = new ClientConfig
        {
            Acks = (Acks) 59
        };

        var value = config.Get("acks");

        await Assert.That((int) config.Acks!).IsEqualTo(59);
        await Assert.That(value).IsEqualTo("59");
    }
}
