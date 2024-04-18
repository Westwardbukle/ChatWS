using Chats.Common.Models;
using MMTR.Http.Facade.Interfaces.ApiClient;
using MMTR.Http.Facade.Models;

namespace Chats.Common.ApiClients.Interfaces
{
    /// <summary>
    /// </summary>
    public interface INotificationApiClient : IApiClientBase
    {
        /// <summary>
        ///     Отправка пуш уведомления по роли
        /// </summary>
        /// <param name="message">Текст уведомления</param>
        /// <param name="roleId">Идентификатор роли пользователя</param>
        /// <param name="redirectUrl">Ссылка для редиректа</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ApiResponse> SendPushByRole(string message, Guid roleId, string? redirectUrl = null,
            CancellationToken token = default);

        /// <summary>
        ///     Отправка пуш уведомления по идентификатору пользователя
        /// </summary>
        /// <param name="message">Текст уведомления</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="theme">Тема уведомления</param>
        /// <param name="redirectUrl">Ссылка для редиректа</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ApiResponse> SendPushByUserId(string message, Guid userId, string? theme = null,
            string? redirectUrl = null,
            CancellationToken token = default);

        /// <summary>
        ///     Отправка пуш уведомлений коллекции пользователей
        /// </summary>
        /// <param name="messages">Идентификаторы пользователей и принадлежащие им сообщения</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ApiResponse> SendPushCollectionUsers(IEnumerable<PushCollectionNotificationRequest> messages,
            CancellationToken token = default);

        /// <summary>
        ///     Отправка Email уведомления пользователю
        /// </summary>
        /// <param name="message">Текст уведомления</param>
        /// <param name="subject">Заголовок письма</param>
        /// <param name="recipients">Электронные почты пользователей</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ApiResponse> SendEmailNotification(IEnumerable<string> recipients, string message, string? subject = null,
            CancellationToken token = default);

        /// <summary>
        ///     Отправка пуш уведомления всем подключенным пользователям
        /// </summary>
        /// <param name="message">Текст уведомления</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ApiResponse> SendPushNotificationAllConnectedUsers(string message, CancellationToken token = default);
    }
}