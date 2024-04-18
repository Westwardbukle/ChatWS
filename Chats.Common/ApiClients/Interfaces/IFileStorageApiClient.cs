using Chats.Common.Files;
using Microsoft.AspNetCore.Http;
using MMTR.Http.Facade.Interfaces.ApiClient;

namespace Chats.Common.ApiClients.Interfaces;

/// <summary>
///     IFileStorageApiClient
/// </summary>
public interface IFileStorageApiClient : IApiClientBase
{
    /// <summary>
    ///     Отправить файл в файлохранилище.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<FileViewModel?> UploadFileAsync(IFormFile file, CancellationToken token = default);
}