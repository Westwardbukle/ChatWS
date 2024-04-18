using System.Text.Json.Serialization;

namespace Chats.Common.Enums
{
    /// <summary>
    /// Типы сообщений
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessageTypes
    {
        /// <summary>
        /// Системное сообщение
        /// </summary>
        System =0,
        /// <summary>
        /// Пользовательское сообщение
        /// </summary>
        User =1
    }
}