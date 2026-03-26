using System.Net;
using Ultimaker.ApiClient.Core;
using Xunit;

namespace Ultimaker.ApiClient.Tests;

public class UltimakerApiResponseTest
{
    [Fact]
    public void Constructor_WithResponseAndMessage_CreatesInstance()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.OK);
        var apiResponse = new UltimakerApiResponse<string>(response, "custom message");

        Assert.Equal("custom message", apiResponse.Message);
        Assert.True(apiResponse.Success);
        Assert.Equal(200, apiResponse.StatusCode);
        Assert.Null(apiResponse.Data);
        Assert.Same(response, apiResponse.RawResponse);
    }
}
