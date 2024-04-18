namespace Chats.Common.Models
{
    /// <summary>
    /// Модель пуш-уведомления при уведомлении коллекции пользователей 
    /// </summary>
    public class PushCollectionNotificationRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Модель пуш уведомления
        /// </summary>
        public PushNotificationRequest MessagesForRecipients { get; set; }
    }
}