using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoMapper;
using Chats.Common.Enums;
using Chats.Common.Filters.Paging;
using Chats.Common.Paging;

namespace Chats.Common.Extentions
{
    /// <summary>
    /// Расширения для работы с постраничным отображением.
    /// </summary>
    public static class PageExtension
    {
        /// <summary>
        /// Получить страницу.
        /// </summary>
        /// <typeparam name="T">Тип источника.</typeparam>
        /// <param name="source">Источник.</param>
        /// <param name="filter">Фильтр.</param>
        /// <param name="order">Функция сортировки.</param>
        /// <param name="direction">направление сортировки.</param>
        /// <returns></returns>
        public static Page<T> ToPage<T>([AllowNull] this IEnumerable<T> source, BasePageFilter? filter = null, Func<T, object>? order = null, OrderDirections? direction = null)
        {
            var page = new Page<T>() { Content = Array.Empty<T>() };

            if (source is null || !source.Any())
                return page;

            if (order is not null)
            {
                if (direction == OrderDirections.Descending)
                    source = source.OrderByDescending(order);
                else if (direction == OrderDirections.Ascending)
                    source = source.OrderBy(order);
            }

            if (filter is null)
                filter = new();

            var isGetCount = source.TryGetNonEnumeratedCount(out var totalElement);
            if (!isGetCount)
                totalElement = source.Count();

            if (filter.Size == 0)
            {
                page.PageSize = totalElement;
                page.PageCount = 1;
                page.Content = source.ToArray();
            }
            else
            {
                var notfullPageCount = totalElement % filter.Size;
                var pageCount = totalElement / filter.Size;

                page.PageSize = filter.Size;
                page.PageCount = notfullPageCount > 0 ? pageCount + 1 : pageCount;
                page.Content = source
                    .Skip(filter.Page * filter.Size)
                    .Take(filter.Size)
                    .ToArray();
            }

            page.PageIndex = filter.Size == 0 ? 0 : filter.Page;
            page.TotalCount = totalElement;

            return page;
        }

        /// <summary>
        /// Получить страницу.
        /// </summary>
        /// <typeparam name="T">Тип источника.</typeparam>
        /// <param name="query">Источник.</param>
        /// <param name="filter">Фильтр.</param>
        /// <param name="order">Функция сортировки.</param>
        /// <param name="direction">направление сортировки.</param>
        /// <returns></returns>
        public static Page<T> ToPage<T>([AllowNull] this IQueryable<T> query, BasePageFilter? filter = null, Expression<Func<T, object>>? order = null, OrderDirections? direction = null)
        {
            var page = new Page<T> { Content = Array.Empty<T>() };

            if (query is null || !query.Any())
                return page;

            if (order is not null)
            {
                if (direction == OrderDirections.Descending)
                    query = query.OrderByDescending(order);
                else if (direction == OrderDirections.Ascending)
                    query = query.OrderBy(order);
            }

            if (filter is null)
                filter = new();

            var isGetCount = query.TryGetNonEnumeratedCount(out var totalElement);
            if (!isGetCount)
                totalElement = query.AsEnumerable().Count();

            if (filter.Size == 0)
            {
                page.PageSize = totalElement;
                page.PageCount = 1;
                page.Content = query.ToArray();
            }
            else
            {
                var notfullPageCount = totalElement % filter.Size;
                var pageCount = totalElement / filter.Size;

                page.PageSize = filter.Size;
                page.PageCount = notfullPageCount > 0 ? pageCount + 1 : pageCount;
                page.Content = query
                    .Skip(filter.Page * filter.Size)
                    .Take(filter.Size)
                    .ToArray();
            }

            page.PageIndex = filter.Size == 0 ? 0 : filter.Page;
            page.TotalCount = totalElement;

            return page;
        }

        /// <summary>
        /// Маппинг страницы одного типа в страницу другого типа
        /// </summary>
        /// <typeparam name="TSource">Тип из которого маппятся данные</typeparam>
        /// <typeparam name="TDestination">Тип в который мапятся данные</typeparam>
        /// <param name="page">Источник</param>
        /// <param name="mapper">Маппер</param>
        /// <returns></returns>
        public static Page<TDestination> Map<TSource, TDestination>(this Page<TSource> page, IMapper mapper)
        {
            return new Page<TDestination>
            {
                PageCount = page.PageCount,
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                TotalCount = page.TotalCount,
                Content = mapper.Map<IEnumerable<TDestination>>(page.Content).ToArray()
            };
        }
    }
}
