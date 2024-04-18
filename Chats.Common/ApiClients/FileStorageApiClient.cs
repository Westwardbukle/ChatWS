using System.Net.Http.Headers;
using Chats.Common.ApiClients.ApiEndpoints;
using Chats.Common.ApiClients.Interfaces;
using Chats.Common.Files;
using Chats.Common.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MMTR.Http.Facade.ApiClient;
using MMTR.Http.Facade.Interfaces.ApiClient;

namespace Chats.Common.ApiClients;

internal class FileStorageApiClient : ApiClientBase<FileStorageApiClient>, IFileStorageApiClient
{
    private readonly IOptions<UriOptions> _options;

    public FileStorageApiClient(IOptions<UriOptions> options, IApiClientContext<FileStorageApiClient> context) : base(context)
    {
        _options = options;
    }

    protected override Uri BaseAddress => new(_options.Value.FileStorage);

    public async Task<FileViewModel?> UploadFileAsync(IFormFile file, CancellationToken token = default)
    {
        var request = await MakeRequestMessageAsync(HttpMethod.Post, FileStorageApiEndpoints.File);

        const string propertyName = "file";

        var content = new StreamContent(file.OpenReadStream());
        content.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
        request.Content = new MultipartFormDataContent
        {
            {content, propertyName, file.FileName}
        };

        var result = await SendAsync<FileViewModel>(request, token);
        return result.Response;
    }
}