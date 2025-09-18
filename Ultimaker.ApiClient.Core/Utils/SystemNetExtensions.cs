using System.Net;

namespace Ultimaker.ApiClient.Core.Utils;

internal static class SystemNetExtensions
{
    internal static bool IsNotFound(this HttpResponseMessage response)
        => response.StatusCode == HttpStatusCode.NotFound;
    
    internal static bool HasCredentials(this NetworkCredential credential)
        => !string.IsNullOrWhiteSpace(credential.UserName) &&
           !string.IsNullOrWhiteSpace(credential.Password);
}