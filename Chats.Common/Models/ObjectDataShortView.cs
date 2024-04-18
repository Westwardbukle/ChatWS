namespace Chats.Common.Models
{
    /// <summary>
    /// Модель объекта, содержащая идентификатор и имя
    /// </summary>
    public class ObjectDataShortView
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        public Guid ObjectId { get; set; }

        /// <summary>
        /// Наименование объекта
        /// </summary>
        public string ObjectName { get; set; } = default!;
    }
}