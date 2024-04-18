namespace Chats.Common.Models
{
    /// <summary>
    /// Модель данных PropertyDefinitionId - Value (для грида, форм и тд)
    /// </summary>
    public class UnoPropertyValue
    {
        /// <summary>
        /// Идентификатор значения свойства
        /// </summary>
        public Guid PropertyDefinitionId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Значение
        /// </summary>
        public object? Value { get; set; } = null!;
    }
}