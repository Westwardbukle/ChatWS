using Chats.Common.Files;

namespace Chats.Domain.Models
{
    /// <summary>
    /// Ответная модель чата
    /// </summary>
    public class ChatShortInfo
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя чата
        /// </summary>
        public string ChatName { get; set; }

        /// <summary>
        /// Описание чата
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Иконка чата
        /// </summary>
        public FileViewModel? Icon { get; set; }

        /// <summary>
        /// Объект в системе, к которому прикреплен чат
        /// </summary>
        public Guid? PinnedObjectId { get; set; }

        /// <summary>
        /// Участники чата
        /// </summary>
        public IEnumerable<UserShortInfo> Members { get; set; }
    }
}