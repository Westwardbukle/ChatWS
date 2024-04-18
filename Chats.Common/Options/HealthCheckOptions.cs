using MMTR.Configuration.Attributes;

namespace Chats.Common.Options;

[AutoConfiguration("HealthCheck")]
public class HealthCheckOptions
{
    /// <summary>
    /// Значение количества попыток подключения к сервису Chats
    /// </summary>
    [AutoConfiguration("CHATS_NUMBEROFCONNECTIONS")]
    public int? Chats { get; set; } = default!;
}