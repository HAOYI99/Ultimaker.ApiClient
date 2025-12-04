using System.Net;
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
        using var httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
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

        // Initially no creds

        client.UpdateCred("newuser", "newpass");

        Assert.NotNull(client.Auth);
        Assert.NotNull(client.Printer);
        // We can't easily check internal state of services, but we verify no exception is thrown
        // and services are re-instantiated.
    }

    [Fact]
    public void UpdateCred_CalledOnClientWithHttpClient_Works()
    {
        using var httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        using var client = new UltimakerClient(httpClient);

        client.UpdateCred("user", "pass");

        Assert.NotNull(client.Auth);
    }
}
