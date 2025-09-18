namespace Ultimaker.ApiClient.Core.Exceptions;

public class MissingCredentialException : InvalidOperationException
{
    public MissingCredentialException() { }
    public MissingCredentialException(string message) : base(message) { }
}