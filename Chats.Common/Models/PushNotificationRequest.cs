namespace Chats.Common.Models
{
    /// <summary>
    /// Модель всплывающего уведомления
    /// </summary>
    public class PushNotificationRequest
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Тема
        /// </summary>
        public string? Theme { get; set; }

        /// <summary>
        /// Ссылка
        /// </summary>
        public string? RedirectUrl { get; set; }
    }
}