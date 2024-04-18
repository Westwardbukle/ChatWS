namespace Chats.Common.Filters
{
    /// <summary>
    /// Фильтр для получения сообщений из чата
    /// </summary>
    public class MessageFilter
    {
        /// <summary>
        /// Идентификатор сообщения, откуда начинается прогрузка
        /// </summary>
        public Guid? MessageId { get; set; }
        
        /// <summary>
        /// В какую сторону прогружаются сообщения
        /// </summary>
        public bool Up { get; set; }
        
        /// <summary>
        /// Количество прогружаемых объектов
        /// </summary>
        public int PageSize { get; set; }
    }
}