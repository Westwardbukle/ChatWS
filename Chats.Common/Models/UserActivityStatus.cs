namespace Chats.Common.Models;

/// <summary>
/// Статус активности пользователя
/// </summary>
public class UserActivityStatus
{
    public UserActivityStatus(bool isActive)
    {
        IsActive = isActive;
    }

    /// <summary>
    /// Активен/не активен
    /// </summary>
    public bool IsActive { get; set; }
}