using Chats.Common.Filters.Paging;
using Chats.Common.Paging;
using Chats.Domain.Models;
using Chats.Domain.Models.Requests;

namespace Chats.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс для взаимодействием с чатами
    /// </summary>
    public interface IChatService
    {

        /// <summary>
        /// Создание чата
        /// </summary>
        /// <param name="request">запрос на создание</param>
        /// <returns></returns>
        Task<ChatShortInfo> CreateChat(CreateChatRequest request);

        /// <summary>
        /// Получение чата по идентификатору
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        Task<ChatShortInfo> GetChatById(Guid chatId);

        /// <summary>
        /// Получение страницы чатов пользователя
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns></returns>
        Task<Page<ChatShortInfo>> GetCurrentUserChatsPage(BasePageFilter filter);

        /// <summary>
        /// Получить чат, прикрепленный к объекту, если нет создается автоматически
        /// </summary>
        /// <param name="classId">Идентификатор объекта</param>
        /// <param name="objectId">Идентификатор объекта</param>
        /// <returns></returns>
        Task<ChatShortInfo> GetPinnedChat(Guid classId, Guid objectId);

        /// <summary>
        /// Обновление чата
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="request">Модель обновления чата</param>
        /// <returns></returns>
        Task<ChatShortInfo> UpdateChat(Guid chatId, CreateChatRequest request);

        /// <summary>
        /// Удаление чата
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        Task<ChatShortInfo> DeleteChat(Guid chatId);
    }
}