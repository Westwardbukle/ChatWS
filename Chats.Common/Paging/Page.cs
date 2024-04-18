namespace Chats.Common.Paging
{
    /// <summary>
    /// Страница контента
    /// </summary>
    /// <typeparam name="T">Модель</typeparam>
    public class Page<T>
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Размер страницы
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Общее количество страниц
        /// </summary>
        public long PageCount { get; set; }

        /// <summary>
        /// Общее количество объектов
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Контент
        /// </summary>
        public T[] Content { get; set; } = default!;
    }
}
