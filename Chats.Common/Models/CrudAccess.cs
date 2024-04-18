namespace Chats.Common.Models
{
    /// <summary>
    /// Список разрешений на CRUD действия
    /// </summary>
    public class CrudAccess
    {
        /// <summary>
        /// Доступно создание
        /// </summary>
        public bool CanCreate { get; set; }

        /// <summary>
        /// Доступно чтение
        /// </summary>
        public bool CanRead { get; set; } = true;

        /// <summary>
        /// Доступно обновление
        /// </summary>
        public bool CanUpdate { get; set; }

        /// <summary>
        /// Доступно удаление
        /// </summary>
        public bool CanDelete { get; set; }
    }
}