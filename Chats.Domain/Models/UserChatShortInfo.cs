namespace Chats.Domain.Models;

/// <summary>
/// Модель пользователя в чате
/// </summary>
public class UserChatShortInfo
{
    /// <summary>
    /// Идентификатор чата
    /// </summary>
    public Guid ChatId { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Когда был привязан пользователь
    /// </summary>
    public DateTime DateCreated { get; set; }
    
    /// <summary>
    /// Когда был отвезян пользователь
    /// </summary>
    public DateTime? DateDeleted { get; set; }
}