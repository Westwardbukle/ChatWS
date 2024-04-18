using System.ComponentModel.DataAnnotations;
using Chats.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Chats.Domain.Models.Requests;

/// <summary>
/// Модель создания сообщения
/// </summary>
public class CreateMessageRequest
{
    /// <summary>
    /// Идентификатор чата, которому принадлежит сообщение
    /// </summary>
    public Guid ChatId { get; set; }
    
    /// <summary>
    /// Идентификаторы упомянутых пользователей
    /// </summary>
    public IEnumerable<Guid>? MentionedUsersIds { get; set; } 

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Тип сообщения
    /// </summary>
    [Required]
    public MessageTypes Type { get; set; } = MessageTypes.User;
    
    /// <summary>
    /// Прикрепленные файлы 
    /// </summary>
    public IFormFileCollection? PinnedFiles { get; set; }
}