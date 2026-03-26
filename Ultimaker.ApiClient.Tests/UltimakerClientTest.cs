using JetBrains.Annotations;
using Ultimaker.ApiClient.Core;

namespace Ultimaker.ApiClient.Tests;

[TestSubject(typeof(UltimakerClient))]
public class UltimakerClientTest
{
    private const string BaseUrl = "http://localhost:8080";

    [Fact]
    public void Constructor_WithHttpClient_InitializedProperties()
    {
        var baseUri = new Uri(BaseUrl);
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = baseUri;
        using var client = new UltimakerClient(httpClient);

        Assert.NotNull(client.Auth);
        Assert.NotNull(client.Material);
        Assert.NotNull(client.Printer);
        Assert.NotNull(client.PrintJob);
        Assert.NotNull(client.System);
        Assert.NotNull(client.History);
        Assert.NotNull(client.AirManager);
    }

    [Fact]
    public void Constructor_WithUrl_InitializedProperties()
    {
        using var client = new UltimakerClient(BaseUrl);

        Assert.NotNull(client.Auth);
        Assert.NotNull(client.Material);
        Assert.NotNull(client.Printer);
        Assert.NotNull(client.PrintJob);
        Assert.NotNull(client.System);
        Assert.NotNull(client.History);
        Assert.NotNull(client.AirManager);
    }

    [Fact]
    public void Constructor_WithCredentials_InitializedProperties()
    {
        using var client = new UltimakerClient(BaseUrl, "user", "pass");

        Assert.NotNull(client.Auth);
        Assert.NotNull(client.Material);
        Assert.NotNull(client.Printer);
        Assert.NotNull(client.PrintJob);
        Assert.NotNull(client.System);
        Assert.NotNull(client.History);
        Assert.NotNull(client.AirManager);
    }

    [Fact]
    public void UpdateCred_UpdatesServicesWithNewCredentials()
    {
        using var client = new UltimakerClient(BaseUrl);
        var previousAuth = client.Auth;
        var previousPrinter = client.Printer;

        client.UpdateCred("newuser", "newpass");

        Assert.NotNull(client.Auth);
        Assert.NotNull(client.Printer);
        Assert.NotSame(previousAuth, client.Auth);
        Assert.NotSame(previousPrinter, client.Printer);
    }

    [Fact]
    public void UpdateCred_CalledOnClientWithHttpClient_Works()
    {
        var baseUri = new Uri(BaseUrl);
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = baseUri;
        using var client = new UltimakerClient(httpClient);
        var previousAuth = client.Auth;

        client.UpdateCred("user", "pass");

        Assert.NotNull(client.Auth);
        Assert.NotSame(previousAuth, client.Auth);
    }
}
