using Chats.Common.Authentication;
using Chats.Common.Filters;
using Chats.Common.Models;
using Chats.Common.Paging;
using MMTR.Http.Facade.Interfaces.ApiClient;

namespace Chats.Common.ApiClients.Interfaces;

/// <summary>
/// </summary>
public interface IAdministrationApiClient : IApiClientBase
{
    Task<UserInfo> GetUserIdentityInfo(CancellationToken token = default);
    /// <summary>
    /// Получить список ролей по id пользователя
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<Page<Role>?> GetRolesByUserId(Guid userId, CancellationToken token = default);

    /// <summary>
    /// </summary>
    /// <param name="roleId">Идентификатор роли</param>
    /// <param name="filter">Фильтр</param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T?> GetUsersByRoleId<T>(Guid roleId, UserGridFilter? filter = null, CancellationToken token = default);


    /// <summary>
    ///     Получить информацию о пользователе.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UserInfo?> GetUserInfoAsync(Guid userId, CancellationToken token = default);

    /// <summary>
    ///     Получение привязанного пользователя к клиенту провайдера аунтентификации
    /// </summary>
    /// <param name="clientId">Идентификатор клиента пользователя</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<ObjectDataShortView?> GetClientLinkedUser(Guid clientId, CancellationToken token = default);

    /// <summary>
    ///     Получить список пользователей со свойствами
    /// </summary>
    /// <param name="filter">Фильтр для настройки запроса</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<Page<UnoGridDataRow>?> GetUsersWithProperties(UserGridFilter? filter = null, CancellationToken token = default);
    
    /// <summary>
    ///     Получение статуса активности пользователя
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UserActivityStatus?> GetUserActivity(Guid userId, CancellationToken token = default);
}