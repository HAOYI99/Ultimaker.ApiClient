using System.Net;
using System.Net.Sockets;
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

    protected Task<UltimakerApiResponse<T?>> GetAsync<T>(string path, CancellationToken ct = default)
    {
        return SendAsyncInternal<T?>(
            sendAction: () => _httpClient.GetAsync(path, ct), ct, checkNotFound: true
        );
    }

    protected Task<UltimakerApiResponse<T?>> PostAsync<T>(string path, HttpContent httpContent,
        CancellationToken ct = default)
    {
        return SendAsyncInternal<T?>(
            sendAction: () => _httpClient.PostAsync(path, httpContent, ct), ct
        );
    }

    protected Task<UltimakerApiResponse<T?>> PutAsync<T>(string path, HttpContent httpContent,
        CancellationToken ct = default)
    {
        return SendAsyncInternal<T?>(
            sendAction: () => _httpClient.PutAsync(path, httpContent, ct), ct
        );
    }

    protected Task<UltimakerApiResponse<T?>> DeleteAsync<T>(string path, CancellationToken ct = default)
    {
        return SendAsyncInternal<T?>(
            sendAction: () => _httpClient.DeleteAsync(path, ct), ct
        );
    }

    protected async Task<UltimakerApiResponse<TData>> SendAsyncInternal<TData>(
        Func<Task<HttpResponseMessage>> sendAction,
        CancellationToken ct = default,
        bool checkNotFound = false,
        bool ensureSuccessStatusCode = true,
        Func<HttpResponseMessage, CancellationToken, Task<TData>>? responseReader = null)
    {
        try
        {
            var response = await sendAction();

            if (checkNotFound && response.IsNotFound())
                return new UltimakerApiResponse<TData>(response);

            if (ensureSuccessStatusCode)
                response.EnsureSuccessStatusCode();

            if (responseReader != null)
            {
                var customData = await responseReader(response, ct);
                return new UltimakerApiResponse<TData>(response, customData);
            }

            var result = await response.Content.ReadAsStringAsync(ct);
            var data = JsonConvert.DeserializeObject<TData>(result, _jsonSetting);
            return new UltimakerApiResponse<TData>(response, data);
        }
        catch (HttpRequestException ex) when (ex.InnerException is SocketException socketEx)
        {
            var message = socketEx.SocketErrorCode switch
            {
                SocketError.HostNotFound =>
                    $"{ex.Message}. It might be incorrect hostname, or the printer is completely offline.",
                _ => ex.Message
            };
            return UltimakerApiResponse<TData>.CustomError(HttpStatusCode.ServiceUnavailable, ex, message);
        }
        catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
        {
            return UltimakerApiResponse<TData>.CustomError(HttpStatusCode.RequestTimeout, ex);
        }
        catch (Exception ex)
        {
            return UltimakerApiResponse<TData>.CustomError(HttpStatusCode.InternalServerError, ex);
        }
    }
}