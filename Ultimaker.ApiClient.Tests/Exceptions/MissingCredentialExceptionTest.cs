using Ultimaker.ApiClient.Core.Exceptions;
using Xunit;

namespace Ultimaker.ApiClient.Tests.Exceptions;

public class MissingCredentialExceptionTest
{
    [Fact]
    public void Constructor_Default_CreatesInstance()
    {
        var exception = new MissingCredentialException();
        Assert.NotNull(exception);
    }

    [Fact]
    public void Constructor_WithMessage_CreatesInstance()
    {
        var exception = new MissingCredentialException("message");
        Assert.Equal("message", exception.Message);
    }
}
