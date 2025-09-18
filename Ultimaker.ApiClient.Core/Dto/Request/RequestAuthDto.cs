namespace Ultimaker.ApiClient.Core.Dto.Request;

public class RequestAuthDto
{
    public string Application { get; set; }
    public string User { get; set; }
    public string? Hostname { get; set; }
    public string? ExclusionKey { get; set; }
}