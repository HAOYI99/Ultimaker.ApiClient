using JetBrains.Annotations;
using Ultimaker.ApiClient.Core.Enums;
using Ultimaker.ApiClient.Core.Utils;
using Xunit;

namespace Ultimaker.ApiClient.Tests.Utils;

[TestSubject(typeof(UltimakerEnumExtensions))]
public class UltimakerEnumExtensionsTest
{
    [Fact]
    public void PrinterVariant_ToOriString_ReturnsCorrectString()
    {
        var variant = PrinterVariant.S5;
        Assert.Equal("Ultimaker S5", variant.ToOriString());
    }

    [Fact]
    public void AirManagerFilterStatus_ToOriString_ReturnsLowerCase()
    {
        var status = AirManagerFilterStatus.REPLACEMENT_REQUIRED;
        Assert.Equal("replacement_required", status.ToOriString());
    }

    [Fact]
    public void AirManagerStatus_ToOriString_ReturnsLowerCase()
    {
        var status = AirManagerStatus.AVAILABLE;
        Assert.Equal("available", status.ToOriString());
    }

    [Fact]
    public void AuthStatus_ToOriString_ReturnsLowerCase()
    {
        var status = AuthStatus.AUTHORIZED;
        Assert.Equal("authorized", status.ToOriString());
    }

    [Fact]
    public void PrinterStatus_ToOriString_ReturnsLowerCase()
    {
        var status = PrinterStatus.PRINTING;
        Assert.Equal("printing", status.ToOriString());
    }

    [Theory]
    [InlineData(JobResult.EMPTY, "")]
    [InlineData(JobResult.FINISHED, "Finished")]
    [InlineData(JobResult.FAILED, "Failed")]
    public void JobResult_ToOriString_ReturnsCorrectFormat(JobResult result, string expected)
    {
        Assert.Equal(expected, result.ToOriString());
    }
}
