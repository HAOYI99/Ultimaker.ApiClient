using System.Net.Mime;
using System.Text;

namespace Ultimaker.ApiClient.Core.Helper;

internal class StringJsonContent : StringContent
{
    private static readonly Encoding DefaultStringEncoding = Encoding.UTF8;
    private const string DEFAULT_MEDIA_TYPE = MediaTypeNames.Application.Json;

    /// <summary>
    /// Creates a JSON HttpContent from any object.
    /// Strings will be wrapped in quotes automatically.
    /// Serialize object using Newtonsoft.Json.
    /// </summary>
    /// <param name="content">Object or string to serialize to JSON</param>
    public StringJsonContent(string content)
        : base(content, DefaultStringEncoding, DEFAULT_MEDIA_TYPE) { }
}