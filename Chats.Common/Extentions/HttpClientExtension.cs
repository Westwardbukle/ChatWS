using System.Web;

namespace Chats.Common.Extentions
{
    /// <summary>
    ///     Расширения для работы с <see cref="HttpClient" />.
    /// </summary>
    public static class HttpClientExtension
    {
        /// <summary>
        ///     Преобразовать объект в строку параметров
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <returns></returns>
        public static string? ToQueryString(this object? obj)
        {
            if (obj is null) return null;
            var properties =
                from p in obj.GetType().GetProperties()
                where p.GetValue(obj, null) != null
                select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }
    }
}