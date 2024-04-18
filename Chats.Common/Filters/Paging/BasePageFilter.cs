namespace Chats.Common.Filters.Paging
{
    /// <summary>
    /// Базовый фильтр для сущностей.
    /// Для гридов объектов необходимо использовать другой фильтр, так как отличается фильтрация/сортировка по полям
    /// </summary>
    public class BasePageFilter
    {
        private int page;

        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page
        {
            get
            {
                return page;
            }
            set
            {
                if (value < 0)
                {
                    page = 0;
                }
                else
                {
                    page = value;
                }
            }
        }

        private int size;

        /// <summary>
        /// Количество объектов на странице (0 - чтобы получить все)
        /// </summary>
        public int Size
        {
            get { return size; }
            set
            {
                if (value < 0)
                {
                    size = 0;
                }
                else
                {
                    size = value;
                }
            }
        }
    }
}
