namespace Chats.Common.Responses
{
    public class MessageResponse
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="MessageResponse"/>
        /// </summary>
        /// <param name="message">Сообщение пользователю/сервису</param>
        public MessageResponse(string message)
        {
            Message = message;
        }
    }
}
