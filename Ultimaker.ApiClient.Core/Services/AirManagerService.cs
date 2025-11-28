using System.Net;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto.Response;

namespace Ultimaker.ApiClient.Core.Services;

public class AirManagerService : ServiceBase
{
    public AirManagerService(HttpClient httpClient) : base(httpClient) { }

    public AirManagerService(HttpClient httpClient, NetworkCredential credential) : base(httpClient, credential) { }

    public Task<UltimakerApiResponse<AirManagerDto?>> Get(CancellationToken ct = default)
        => GetAsync<AirManagerDto>(UltimakerPaths.AirManager.Base, ct);
}