using System.Net;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Core;

public class UltimakerClient
{
    private readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(10);

    public AuthService Auth { get; internal set; }
    public MaterialService Material { get; internal set; }
    public PrinterService Printer { get; internal set; }
    public PrintJobService PrintJob { get; internal set; }
    public SystemService System { get; internal set; }
    public HistoryService History { get; internal set; }
    public AirManagerService AirManager { get; internal set; }

    public UltimakerClient(string url)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(url),
            Timeout = DEFAULT_TIMEOUT
        };
        Auth = new AuthService(httpClient);
        Material = new MaterialService(httpClient);
        Printer = new PrinterService(httpClient);
        PrintJob = new PrintJobService(httpClient);
        System = new SystemService(httpClient);
        History = new HistoryService(httpClient);
        AirManager = new AirManagerService(httpClient);
    }

    public UltimakerClient(string url, string username, string password)
    {
        var credential = new NetworkCredential(username, password);
        var handler = new HttpClientHandler { Credentials = credential };
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(url),
            Timeout = DEFAULT_TIMEOUT
        };
        Auth = new AuthService(httpClient, credential);
        Material = new MaterialService(httpClient, credential);
        Printer = new PrinterService(httpClient, credential);
        PrintJob = new PrintJobService(httpClient, credential);
        System = new SystemService(httpClient, credential);
        History = new HistoryService(httpClient, credential);
        AirManager = new AirManagerService(httpClient, credential);
    }
}