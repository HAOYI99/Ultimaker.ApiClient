namespace Ultimaker.ApiClient.Core.Dto;

public class FileItem(byte[] content, string fileName)
{
    public byte[] Content { get; set; } = content;
    public string FileName { get; set; } = fileName;
    public string FileNameOnly => Path.GetFileNameWithoutExtension(FileName);
}