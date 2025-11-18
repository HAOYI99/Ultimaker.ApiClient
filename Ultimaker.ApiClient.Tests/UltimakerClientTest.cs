using JetBrains.Annotations;
using Ultimaker.ApiClient.Core;

namespace Ultimaker.ApiClient.Tests;

[TestSubject(typeof(UltimakerClient))]
public class UltimakerClientTest
{
    [Fact]
    public void InitService()
    {
        var client = new UltimakerClient("http://localhost");
        Assert.NotNull(client.Auth);
        Assert.NotNull(client.Material);
        Assert.NotNull(client.Printer);
        Assert.NotNull(client.PrintJob);
        Assert.NotNull(client.System);
        Assert.NotNull(client.History);
        Assert.NotNull(client.AirManager);
        client.Dispose();
    }

    [Fact]
    public void InitService_WithCred()
    {
        var client = new UltimakerClient("http://localhost", "user", "pass");
        Assert.NotNull(client.Auth);
        Assert.NotNull(client.Material);
        Assert.NotNull(client.Printer);
        Assert.NotNull(client.PrintJob);
        Assert.NotNull(client.System);
        Assert.NotNull(client.History);
        Assert.NotNull(client.AirManager);
        client.Dispose();
    }
}