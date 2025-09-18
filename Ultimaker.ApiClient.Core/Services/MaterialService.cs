using System.Net;
using Ultimaker.ApiClient.Core.Constants;
using Ultimaker.ApiClient.Core.Dto;
using Ultimaker.ApiClient.Core.Dto.Response;
using Ultimaker.ApiClient.Core.Helper;

namespace Ultimaker.ApiClient.Core.Services;

public class MaterialService : ServiceBase
{
    public MaterialService(HttpClient httpClient) : base(httpClient) { }
    public MaterialService(HttpClient httpClient, NetworkCredential credential) : base(httpClient, credential) { }

    public Task<string[]?> GetAll(CancellationToken ct = default)
        => GetAsync<string[]>(UltimakerPaths.Material.Base, ct);

    public Task<ResultDto?> Upload(FileItem materialFile, CancellationToken ct = default)
        => PostFileAsync(UltimakerPaths.Material.Base, materialFile, ct);

    public Task<string?> GetById(Guid id, CancellationToken ct = default)
        => GetAsync<string>(UltimakerPaths.Material.IdPath(id), ct);

    public Task<ResultDto?> DeleteById(Guid id, CancellationToken ct = default)
        => DeleteAsync<ResultDto>(UltimakerPaths.Material.IdPath(id), ct);


    private async Task<ResultDto?> PostFileAsync(string path, FileItem file, CancellationToken ct = default)
    {
        var formData = new MultipartFormBuilder()
            .AddString("filename", file.FileName)
            .AddFile("file", file.Content, file.FileName)
            .Build();
        return await PostAsync<ResultDto>(path, formData, ct);
    }
}