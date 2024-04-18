using System.ComponentModel.DataAnnotations.Schema;
using Chats.Common.Enums;
using Chats.Common.Files;
using Chats.Common.Models;

namespace Chats.Database.Entities
{
    /// <summary>
    /// Модель БД сообщения чата 
    /// </summary>
    [Table("Message")]
    public class MessageEntity : BaseEntity
    {
        /// <summary>
        /// Идентификатор чата, которому принадлежит сообщение
        /// </summary>
        public Guid ChatId { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит сообщение
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Идентификаторы упомянутых пользователей
        /// </summary>
        [Column(TypeName = "jsonb")]
        public IEnumerable<Guid>? MentionedUsersIds { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Тип сообщения
        /// </summary>
        public MessageTypes Type { get; set; }

        /// <summary>
        /// Прикрепленные файлы к сообщению
        /// </summary>
        [Column(TypeName = "jsonb")]
        public IEnumerable<FileViewModel>? PinnedFiles { get; set; }

        [ForeignKey(nameof(ChatId))]
        public ChatEntity Chat { get; set; }

        public Guid[] ReadBy { get; set; } 
    }
}