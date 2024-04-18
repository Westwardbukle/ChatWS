using MMTR.Configuration.Attributes;

namespace Chats.Common.Options
{
    [AutoConfiguration]
    public class MinioOptions
    {
        [AutoConfiguration("Minio:DocumentValidExtensions")]
        public string DocumentValidExtensions { get; set; }
    }
}