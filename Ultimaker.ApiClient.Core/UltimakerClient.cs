using System.Net;
using Ultimaker.ApiClient.Core.Services;

namespace Ultimaker.ApiClient.Core;

public class UltimakerClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(10);

    public AuthService Auth { get; internal set; }
    public MaterialService Material { get; internal set; }
    public PrinterService Printer { get; internal set; }
    public PrintJobService PrintJob { get; internal set; }
    public SystemService System { get; internal set; }
    public HistoryService History { get; internal set; }
    public AirManagerService AirManager { get; internal set; }

    public UltimakerClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        Auth = new AuthService(_httpClient);
        Material = new MaterialService(_httpClient);
        Printer = new PrinterService(_httpClient);
        PrintJob = new PrintJobService(_httpClient);
        System = new SystemService(_httpClient);
        History = new HistoryService(_httpClient);
        AirManager = new AirManagerService(_httpClient);
    }
    
    public UltimakerClient(string url)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(url),
            Timeout = DEFAULT_TIMEOUT
        };
        Auth = new AuthService(_httpClient);
        Material = new MaterialService(_httpClient);
        Printer = new PrinterService(_httpClient);
        PrintJob = new PrintJobService(_httpClient);
        System = new SystemService(_httpClient);
        History = new HistoryService(_httpClient);
        AirManager = new AirManagerService(_httpClient);
    }

    public UltimakerClient(string url, string username, string password)
    {
        var credential = new NetworkCredential(username, password);
        var handler = new HttpClientHandler { Credentials = credential };
        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(url),
            Timeout = DEFAULT_TIMEOUT
        };
        Auth = new AuthService(_httpClient, credential);
        Material = new MaterialService(_httpClient, credential);
        Printer = new PrinterService(_httpClient, credential);
        PrintJob = new PrintJobService(_httpClient, credential);
        System = new SystemService(_httpClient, credential);
        History = new HistoryService(_httpClient, credential);
        AirManager = new AirManagerService(_httpClient, credential);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}