using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

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
    public StringJsonContent(object content)
        : base(JsonConvert.SerializeObject(content), DefaultStringEncoding, DEFAULT_MEDIA_TYPE) { }

    public StringJsonContent(object content, JsonSerializerSettings settings)
        : base(JsonConvert.SerializeObject(content, settings), DefaultStringEncoding, DEFAULT_MEDIA_TYPE) { }
}