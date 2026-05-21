using System.Net;

namespace Ultimaker.ApiClient.Core;

public class UltimakerApiResponse<TData>
{
    public TData? Data { get; init; }
    public bool Success { get; init; }
    public string Message { get; init; }
    public int StatusCode { get; init; }

    public HttpResponseMessage? RawResponse { get; init; }
    public Exception? Exception { get; init; }

    public UltimakerApiResponse(HttpResponseMessage response, TData? data, string? message = null)
    {
        RawResponse = response ?? throw new ArgumentNullException(nameof(response));
        Data = data;
        Success = response.IsSuccessStatusCode;
        StatusCode = (int)response.StatusCode;
        Message = message ?? response.ReasonPhrase ?? string.Empty;
    }

    public UltimakerApiResponse(HttpResponseMessage response, string? message)
        : this(response, default, message) { }

    public UltimakerApiResponse(HttpResponseMessage response)
        : this(response, default, string.Empty) { }


    private UltimakerApiResponse(HttpStatusCode statusCode, string message, Exception? ex)
    {
        Success = false;
        StatusCode = (int)statusCode;
        Message = message;
        Exception = ex;
        RawResponse = null;
        Data = default;
    }

    public static UltimakerApiResponse<TData> CustomError(HttpStatusCode statusCode, Exception ex)
    {
        return new UltimakerApiResponse<TData>(statusCode, ex.Message, ex);
    }

    public static UltimakerApiResponse<TData> CustomError(HttpStatusCode statusCode, Exception ex, string message)
    {
        return new UltimakerApiResponse<TData>(statusCode, message, ex);
    }
}