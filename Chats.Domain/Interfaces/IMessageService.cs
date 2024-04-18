using Chats.Common.Filters;
using Chats.Domain.Models;
using Chats.Domain.Models.Requests;

namespace Chats.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс для взаимодействия с сервисом сообщений
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Создание сообщения
        /// </summary>
        /// <param name="request">Модель запроса на осздание</param>
        /// <returns></returns>
        Task<MessageShortInfo> CreateMessage(CreateMessageRequest request);

        /// <summary>
        /// Получить страницу сообщений из чата
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="filter">Фильтр</param>
        /// <returns></returns>
        Task<MessageInChatInfo> GetPageMessagesFromChat(Guid chatId, MessageFilter filter);

        /// <summary>
        /// Удаление сообщения
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения</param>
        /// <returns></returns>
        Task<MessageShortInfo> DeleteMessage(Guid messageId);

        /// <summary>
        /// Прочтение всех сообщений пользователей в чате, до определенного
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="messageId">Идентификатор сообщения</param>
        /// <returns>Оставшееся количество непрочитанных сообщений</returns>
        Task<int> ReadMessagesUpToSpecificMessage(Guid chatId, Guid? messageId);
    }
}