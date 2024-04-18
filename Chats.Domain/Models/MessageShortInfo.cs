using Chats.Common.Enums;
using Chats.Common.Files;

namespace Chats.Domain.Models;

/// <summary>
/// Возвращаемая модель сообщения
/// </summary>
public class MessageShortInfo
{
    /// <summary>
    /// Идентификатор чата
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор чата, которому принадлежит сообщение
    /// </summary>
    public Guid ChatId { get; set; }
    
    /// <summary>
    /// Идентификаторы упомянутых пользователей
    /// </summary>
    public IEnumerable<Guid>? MentionedUsersIds { get; set; }

    /// <summary>
    /// Идентификатор пользователя, которому принадлежит сообщение
    /// </summary>
    public UserShortInfo User { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Тип сообщения
    /// </summary>
    public MessageTypes Type { get; set; }

    /// <summary>
    /// Прикрепленные файлы к сообщению
    /// </summary>
    public IEnumerable<FileViewModel>? PinnedFiles { get; set; }
    
    /// <summary>
    /// Дата создания сообщения
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Идентификаторы людей, прочитавших сообщение
    /// </summary>
    public bool ReadByCurrentUser { get; set; }
}