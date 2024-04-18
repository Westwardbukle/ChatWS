
using MMTR.Configuration.Attributes;

namespace Chats.Common.Options
{
    /// <summary>
    /// Параметры Uri
    /// </summary>
    [AutoConfiguration("Uri")]
    public class UriOptions
    {
        /// <summary>
        /// Базовый адрес сервиса UNO
        /// </summary>
        [AutoConfiguration("UNO_URL")]
        public string Uno { get; set; } = default!;
        
        /// <summary>
        /// Базовый адрес сервиса файлового хранилища
        /// </summary>
        [AutoConfiguration("FILES_URL")]
        public string FileStorage { get; set; } = default!;

        /// <summary>
        /// Базовый адрес сервиса администрирования
        /// </summary>
        [AutoConfiguration("ADMIN_URL")]
        public string Administration { get; set; } = default!;

        /// <summary>
        /// Базовый адреса сервиса уведомлений
        /// </summary>
        [AutoConfiguration("NOTIFICATIONS_URL")]
        public string Notification { get; set; } = default!;

        /// <summary>
        /// Параметры Uri фронта
        /// </summary>
        [AutoConfiguration("FRONT_URL")]
        public string Frontend { get; set; } = default!;
    }
}