using Chats.Common.Files;

namespace Chats.Domain.Models.Requests
{
    /// <summary>
    /// Модель запроса создания чата
    /// </summary>
    public class CreateChatRequest
    {
        /// <summary>
        /// Имя чата
        /// </summary>
        public string ChatName { get; set; } = default!;
    
        /// <summary>
        /// Описание чата
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Иконка чата
        /// </summary>
        public FileViewModel? Icon { get; set; }

        /// <summary>
        /// Класс в системе, к которому прикреплен чат
        /// </summary>
        public Guid? PinnedClassId { get; set; }

        /// <summary>
        /// Объект в системе, к которому прикреплен чат
        /// </summary>
        public Guid? PinnedObjectId { get; set; }
    }
}