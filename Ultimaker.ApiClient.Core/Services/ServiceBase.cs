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

    protected async Task<T?> GetAsync<T>(string path, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync(path, ct);
        if (response.IsNotFound())
            return default;
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(ct);
        return JsonConvert.DeserializeObject<T>(result, _jsonSetting);
    }

    protected async Task<T?> PostAsync<T>(string path, HttpContent httpContent, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsync(path, httpContent, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(ct);
        return JsonConvert.DeserializeObject<T>(result, _jsonSetting);
    }

    protected async Task<T?> PutAsync<T>(string path, HttpContent httpContent, CancellationToken ct = default)
    {
        var response = await _httpClient.PutAsync(path, httpContent, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(ct);
        return JsonConvert.DeserializeObject<T>(result, _jsonSetting);
    }

    protected async Task<T?> DeleteAsync<T>(string path, CancellationToken ct = default)
    {
        var response = await _httpClient.DeleteAsync(path, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(ct);
        return JsonConvert.DeserializeObject<T>(result, _jsonSetting);
    }
}