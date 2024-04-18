namespace Chats.Domain.Models
{
    /// <summary>
    /// Короткая модель о пользователе
    /// </summary>
    public class UserShortInfo
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Имя пользователя в системе
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ссылка на миниатюрную аватарку
        /// </summary>
        public string? MiniatureLink { get; set; }
    }
}