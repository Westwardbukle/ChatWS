using Chats.Domain.Interfaces;
using Chats.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chats.Application.Controllers
{
    /// <summary>
    /// Контроллер для взаимодействия с подписками
    /// </summary>
    [ApiController]
    [Route("api/subscribe")]
    [Authorize]
    public class SubscribeController : ControllerBase
    {
        private readonly IUsersInteractionsChatsService _usersInteractions;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="usersInteractions"></param>
        public SubscribeController(IUsersInteractionsChatsService usersInteractions)
        {
            _usersInteractions = usersInteractions;
        }

        /// <summary>
        /// Получить информацию о том, подписан ли текущий пользователь на чат
        /// </summary>
        /// <returns></returns>
        [HttpGet("my")]
        public async Task<ActionResult<ChatShortInfo>> CheckCurrentUserSubscription()
        {
            var result = await _usersInteractions.GetCurrentUserSubscriptions();
            return Ok(result);
        }

        /// <summary>
        /// Подписать текущего пользователя на чат
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        [HttpPost("{chatId:guid}")]
        public async Task<ActionResult<UserChatShortInfo>> Subscribe(Guid chatId)
        {
            await _usersInteractions.SubscribeCurrentUserToChat(chatId);
            return Ok();
        }

        /// <summary>
        /// Отписать текущего пользователя от чата
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        [HttpDelete("{chatId:guid}")]
        public async Task<ActionResult<UserChatShortInfo>> Unsubscribe(Guid chatId)
        {
            await _usersInteractions.UnsubscribeCurrentUserToChat(chatId);
            return Ok();
        }

        /// <summary>
        /// Получить информацию о том, подписан ли текущий пользователь на чат
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        [HttpGet("{chatId:guid}/me")]
        public async Task<ActionResult<bool>> CheckCurrentUserSubscription(Guid chatId)
        {
            var result = await _usersInteractions.CheckCurrentUserSubscription(chatId);
            return Ok(result);
        }

        /// <summary>
        /// Получить пользователей, подписанных на уведомления
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        [HttpGet("{chatId:guid}")]
        public async Task<ActionResult<IEnumerable<UserChatShortInfo>>> GetChatSubscribers(Guid chatId)
        {
            var result = await _usersInteractions.GetChatSubscribers(chatId);
            return Ok(result);
        }
    }
}