using Chats.Common;
using Chats.Common.Filters;
using Chats.Domain.Interfaces;
using Chats.Domain.Models;
using Chats.Domain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chats.Application.Controllers
{
    /// <summary>
    /// Контроллер для сообщений
    /// </summary>
    [ApiController]
    [Route("api/messages")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="messageService"></param>
        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Создание сообщения в чате
        /// </summary>
        /// <param name="request">Модель запроса создания сообщения</param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = Constants.MultipartBodyLengthLimit50MB)]
        public async Task<ActionResult<MessageShortInfo>> CreateMessage([FromForm] CreateMessageRequest request)
        {
            var result = await _messageService.CreateMessage(request);
            return Ok(result);
        }

        /// <summary>
        /// Получение страницы сообщений определенного чата
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="filter">Идентификатор пользователя</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<MessageShortInfo>> GetPageMessagesFromChat(
            [FromQuery] Guid chatId,
            [FromQuery] MessageFilter filter)
        {
            var result = await _messageService.GetPageMessagesFromChat(chatId, filter);
            return Ok(result);
        }

        /// <summary>
        /// Удаление сообщения
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения</param>
        /// <returns></returns>
        [HttpDelete("{messageId:guid}")]
        public async Task<ActionResult<MessageShortInfo>> DeleteMessage(Guid messageId)
        {
            var result = await _messageService.DeleteMessage(messageId);
            return Ok(result);
        }

        /// <summary>
        /// Прочтение всех сообщений пользователем по определенное сообщение
        /// </summary>
        /// <param name="chatId"> Идентификатор чата</param>
        /// <param name="messageId">Идентификатор сообщения</param>
        /// <returns></returns>
        [HttpPut("read/{chatId:guid}")]
        public async Task<ActionResult> ReadAllMessagesUpTo(Guid chatId, 
            [FromQuery] Guid? messageId)
        {
            var result = await _messageService.ReadMessagesUpToSpecificMessage(chatId, messageId);
            return Ok(result);
        }
    }
}