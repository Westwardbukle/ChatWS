using System.ComponentModel.DataAnnotations.Schema;
using Chats.Common.Files;
using Chats.Common.Models;

namespace Chats.Database.Entities
{
    /// <summary>
    /// Модель БД чата
    /// </summary>
    [Table("Chat")]
    public class ChatEntity : BaseEntity
    {
        /// <summary>
        /// Имя чата
        /// </summary>
        public string ChatName { get; set; }
    
        /// <summary>
        /// Описание чата
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Инфа о иконке чата
        /// </summary>
        [Column(TypeName = "jsonb")]
        public FileViewModel? Icon { get; set; }

        /// <summary>
        /// Класс в системе, к которому прикреплен чат
        /// </summary>
        public Guid? PinnedClassId { get; set; }

        /// <summary>
        /// Объект в системе, к которому прикреплен чат
        /// </summary>
        public Guid? PinnedObjectId { get; set; }
        
        public ICollection<UserChatEntity> UserChats { get; set; }

        public ICollection<MessageEntity> Messages { get; set; }
    }
}