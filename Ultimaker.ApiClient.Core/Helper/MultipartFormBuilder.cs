using System.Net.Http.Headers;
using Ultimaker.ApiClient.Core.Dto;

namespace Ultimaker.ApiClient.Core.Helper;

internal class MultipartFormBuilder
{
    private readonly MultipartFormDataContent _form = new();
    
    internal MultipartFormBuilder AddString(string name, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _form.Add(new StringContent(value), name);
        }
        return this;
    }

    internal MultipartFormBuilder AddFile(string name, byte[] content, string filename)
    {
        if (content.Length == 0) return this;
        
        var fileContent = new ByteArrayContent(content);
        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = $"\"{name}\"",
            FileName = $"\"{filename}\""
        };
        _form.Add(fileContent, name, filename);
        return this;
    }

    internal MultipartFormBuilder AddFile(string name, FileItem file)
    {
        AddFile(name, file.Content, file.FileName);
        return this;
    }
    
    internal MultipartFormDataContent Build() => _form;
}