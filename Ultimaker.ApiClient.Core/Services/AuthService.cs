using System.Net;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto.Request;
using Ultimaker.ApiClient.Core.Dto.Response;
using Ultimaker.ApiClient.Core.Helper;

namespace Ultimaker.ApiClient.Core.Services;

public class AuthService : ServiceBase
{
    public AuthService(HttpClient httpClient) : base(httpClient) { }
    public AuthService(HttpClient httpClient, NetworkCredential credential) : base(httpClient, credential) { }

    public Task<AuthDto?> Request(RequestAuthDto request, CancellationToken ct = default)
        => RequestAuthAsync(UltimakerPaths.Auth.Request, request, ct);

    public Task<CheckAuthDto?> Check(string id, CancellationToken ct = default)
        => GetAsync<CheckAuthDto>(UltimakerPaths.Auth.Check(id), ct);

    public Task<HttpStatusCode> Verify(CancellationToken ct = default)
        => VerifyAsync(UltimakerPaths.Auth.Verify, ct);


    private async Task<AuthDto?> RequestAuthAsync(string path, RequestAuthDto request, CancellationToken ct = default)
    {
        var formData = new MultipartFormBuilder()
            .AddString("application", request.Application)
            .AddString("user", request.User)
            .AddString("host_name", request.Hostname)
            .AddString("exclusion_key", request.ExclusionKey)
            .Build();
        return await PostAsync<AuthDto>(path, formData, ct);
    }

    private async Task<HttpStatusCode> VerifyAsync(string path, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync(path, ct);
        return response.StatusCode;
    }
}