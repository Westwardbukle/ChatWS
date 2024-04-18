using Chats.Common.Filters.Paging;

namespace Chats.Common.Filters
{
    /// <summary>
    /// Фильтр списка сотрудников.
    /// </summary>
    public class UserGridFilter : BasePageFilter
    {
        /// <summary>
        /// Список определений свойств, которые требуется получить от сотрудника.
        /// </summary>
        public IEnumerable<Guid>? PropertyDefinitionIds { get; set; }

        /// <summary>
        /// Список объектов, которые не требуется отображать.
        /// </summary>
        public IEnumerable<Guid>? ExludedObjects { get; set; }

        /// <summary>
        /// Отображать только активных пользователей.
        /// </summary>
        public bool OnlyActive { get; set; } = false;
    }
}
