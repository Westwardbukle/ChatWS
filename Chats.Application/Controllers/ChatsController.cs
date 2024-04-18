using System.ComponentModel.DataAnnotations;
using Chats.Common.Filters.Paging;
using Chats.Common.Paging;
using Chats.Domain.Interfaces;
using Chats.Domain.Models;
using Chats.Domain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chats.Application.Controllers
{
    /// <summary>
    /// Контроллер для взаимодействия с чатами
    /// </summary>
    [ApiController]
    [Route("api/chats")]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IUsersInteractionsChatsService _usersInteractions;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ChatsController(
            IChatService chatService,
            IUsersInteractionsChatsService usersInteractions)
        {
            _chatService = chatService;
            _usersInteractions = usersInteractions;
        }

        /// <summary>
        /// Создание чата
        /// </summary>
        /// <param name="request">Модель запроса создания чата </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ChatShortInfo>> CreateChat(
            [FromBody] CreateChatRequest request)
        {
            var result = await _chatService.CreateChat(request);
            return Ok(result);
        }

        /// <summary>
        /// Получение чата по идентификатору
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        [HttpGet("{chatId:guid}")]
        public async Task<ActionResult<ChatShortInfo>> GetChatById(Guid chatId)
        {
            var result = await _chatService.GetChatById(chatId);
            return Ok(result);
        }

        /// <summary>
        /// Обновление чата
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="request">Модель запроса обновления</param>
        /// <returns></returns>
        [HttpPut("{chatId:guid}")]
        public async Task<ActionResult<ChatShortInfo>> UpdateChat(Guid chatId, [FromBody] CreateChatRequest request)
        {
            var result = await _chatService.UpdateChat(chatId, request);
            return Ok(result);
        }

        /// <summary>
        /// Удаление чата
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        [HttpDelete("{chatId:guid}")]
        public async Task<ActionResult<ChatShortInfo>> DeleteChat(Guid chatId)
        {
            var result = await _chatService.DeleteChat(chatId);
            return Ok(result);
        }

        /// <summary>
        /// Получение страницы чатов пользователя
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns></returns>
        [HttpGet("my")]
        public async Task<ActionResult<Page<ChatShortInfo>>> GetCurrentUserChatsPage(
            [FromQuery] BasePageFilter filter)
        {
            var result = await _chatService.GetCurrentUserChatsPage(filter);
            return Ok(result);
        }

        /// <summary>
        /// Получение прикрепленного чата к объекту
        /// </summary>
        /// <param name="classId">Идентификатор класса</param>
        /// <param name="objectId">Идентификатор объекта</param>
        /// <returns></returns>
        [HttpGet("pinned")]
        public async Task<ActionResult<IEnumerable<ChatShortInfo>>> GetPinnedChat(
            [FromQuery][Required] Guid classId,
            [FromQuery][Required] Guid objectId)
        {
            var result = await _chatService.GetPinnedChat(classId, objectId);
            return Ok(result);
        }

        /// <summary>
        /// Получить пользователей чата
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        [HttpGet("{chatId:guid}/members")]
        public async Task<ActionResult<IEnumerable<UserChatShortInfo>>> GetChatMembers(Guid chatId)
        {
            var result = await _usersInteractions.GetChatMembers(chatId);
            return Ok(result);
        }
    }
}