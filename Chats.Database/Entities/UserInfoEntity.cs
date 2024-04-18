using System.ComponentModel.DataAnnotations.Schema;
using Chats.Common.Models;

namespace Chats.Database.Entities
{
    /// <summary>
    /// Модель инфы о пользователе в БД
    /// </summary>
    [Table("UserInfo")]
    public class UserInfoEntity : BaseEntity
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Имя пользователя взятое из UNO
        /// </summary>
        public string Name { get; set; }
        
        public ICollection<UserChatEntity> UserChat { get; set; }

        public ICollection<SubscribeEntity> Subscriptions { get; set; }
    }
}