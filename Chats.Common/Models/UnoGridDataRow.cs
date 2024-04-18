namespace Chats.Common.Models
{
    /// <summary>
    /// Модель строки данных для таблицы
    /// </summary>
    public class UnoGridDataRow
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        public Guid ObjectId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Наименование объекта
        /// </summary>
        public string ObjectName { get; set; } = default!;

        /// <summary>
        /// Ссылка на объект, который скопировали
        /// </summary>
        public Guid? ReferenceDataSourceId { get; set; } = default!;

        /// <summary>
        /// Идентификатор определения свойства, при группировке
        /// </summary>
        public Guid? GroupedBy { get; set; } = default!;

        /// <summary>
        /// Массив свойств объекта
        /// </summary>
        public UnoPropertyValue[] Properties { get; set; } = Array.Empty<UnoPropertyValue>();
    }
}