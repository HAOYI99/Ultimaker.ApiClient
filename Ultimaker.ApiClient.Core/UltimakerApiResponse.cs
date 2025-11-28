namespace Ultimaker.ApiClient.Core;

public class UltimakerApiResponse<TData>
{
    public TData? Data { get; init; }
    public bool Success { get; init; }
    public string Message { get; init; }
    public int StatusCode { get; init; }
    public HttpResponseMessage RawResponse { get; init; }

    public UltimakerApiResponse(HttpResponseMessage response, TData? data, string? message = null)
    {
        RawResponse = response ?? throw new ArgumentNullException(nameof(response));
        Data = data;
        Success = response.IsSuccessStatusCode;
        StatusCode = (int)response.StatusCode;
        Message = message ?? response.ReasonPhrase ?? "";
    }

    public UltimakerApiResponse(HttpResponseMessage response, string? message)
        : this(response, default, message) { }
    
    public UltimakerApiResponse(HttpResponseMessage response)
        : this(response, default, null) { }
}