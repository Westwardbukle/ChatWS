using Chats.Domain.Models;

namespace Chats.Domain.Interfaces
{
    /// <summary>
    /// Взаимодействие с пользователями
    /// </summary>
    public interface IUsersInteractionsChatsService
    {
        /// <summary>
        /// Присоединить текущего пользователя к чату
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task ConnectCurrentUserToChat(Guid chatId);

        /// <summary>
        /// Отсоединить текущего пользователя от чата
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task DisconnectCurrentUserToChat(Guid chatId);

        /// <summary>
        /// Получить пользователей, подключенных к чату
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task<IEnumerable<UserShortInfo>> GetChatMembers(Guid chatId);

        /// <summary>
        /// Подписать текущего пользователя на уведомления чата
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task SubscribeCurrentUserToChat(Guid chatId);

        /// <summary>
        /// Отписать текущего пользователя от уведомлений чата
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task UnsubscribeCurrentUserToChat(Guid chatId);

        /// <summary>
        /// Получить подписки текущего пользователя
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ChatShortInfo>> GetCurrentUserSubscriptions();

        /// <summary>
        /// Проверить подписку текущего пользователя на чат
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task<bool> CheckCurrentUserSubscription(Guid chatId);

        /// <summary>
        /// Получить пользователей, подписанных на чат
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task<IEnumerable<UserShortInfo>> GetChatSubscribers(Guid chatId);

        /// <summary>
        /// Получить сохраненную информацию о текущем пользователе
        /// </summary>
        /// <returns></returns>
        Task<UserShortInfo> GetCurrentUserInfo();
    }
}