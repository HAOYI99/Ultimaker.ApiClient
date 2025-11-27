using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ultimaker.ApiClient.Core.Exceptions;
using Ultimaker.ApiClient.Core.Utils;

namespace Ultimaker.ApiClient.Core.Services;

public abstract class ServiceBase
{
    protected readonly HttpClient _httpClient;
    protected readonly NetworkCredential _credential;

    protected readonly JsonSerializerSettings _jsonSetting = new()
    {
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        Converters = { new StringEnumConverter() }
    };

    protected ServiceBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _credential = new NetworkCredential();
    }

    protected ServiceBase(HttpClient httpClient, NetworkCredential credential)
    {
        _httpClient = httpClient;
        _credential = credential;
    }

    protected void EnsureHasCredential()
    {
        if (!_credential.HasCredentials())
            throw new MissingCredentialException("Credential is not set.");
    }

    protected async Task<UltimakerApiResponse<T?>> GetAsync<T>(string path, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync(path, ct);
        if (response.IsNotFound())
            return new UltimakerApiResponse<T?>(response);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(ct);
        var data = JsonConvert.DeserializeObject<T>(result, _jsonSetting);
        return new UltimakerApiResponse<T?>(response, data);
    }

    protected async Task<UltimakerApiResponse<T?>> PostAsync<T>(string path, HttpContent httpContent, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsync(path, httpContent, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(ct);
        var data = JsonConvert.DeserializeObject<T>(result, _jsonSetting);
        return new UltimakerApiResponse<T?>(response, data);
    }

    protected async Task<UltimakerApiResponse<T?>> PutAsync<T>(string path, HttpContent httpContent, CancellationToken ct = default)
    {
        var response = await _httpClient.PutAsync(path, httpContent, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(ct);
        var data = JsonConvert.DeserializeObject<T>(result, _jsonSetting);
        return new UltimakerApiResponse<T?>(response, data);
    }

    protected async Task<UltimakerApiResponse<T?>> DeleteAsync<T>(string path, CancellationToken ct = default)
    {
        var response = await _httpClient.DeleteAsync(path, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(ct);
        var data = JsonConvert.DeserializeObject<T>(result, _jsonSetting);
        return new UltimakerApiResponse<T?>(response, data);
    }
}