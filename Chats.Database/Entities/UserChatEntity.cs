using System.ComponentModel.DataAnnotations.Schema;
using Chats.Common.Models;

namespace Chats.Database.Entities
{
    /// <summary>
    /// Модель привязки пользователя к чату
    /// </summary>
    [Table("UserChat")]
    public class UserChatEntity : BaseEntity
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public Guid ChatId { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        [ForeignKey(nameof(ChatId))] 
        public ChatEntity Chat { get; set; }
        
        public UserInfoEntity UserInfo { get; set; }
    }
}