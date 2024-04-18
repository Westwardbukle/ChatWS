namespace Chats.Domain.Models
{
    /// <summary>
    /// Модель с информацией о сообщениях в чате
    /// </summary>
    public class MessageInChatInfo
    {
        /// <summary>
        /// Сообщения в чате
        /// </summary>
        public IEnumerable<MessageShortInfo> Messages { get; set; }

        /// <summary>
        /// Количество не проичтанных сообщений
        /// </summary>
        public int CountUnread { get; set; }
    }
}