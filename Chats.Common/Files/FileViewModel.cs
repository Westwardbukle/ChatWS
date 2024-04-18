namespace Chats.Common.Files
{
    /// <summary>
    /// Модель создаваемого файла
    /// </summary>
    public class FileViewModel : FileInfoShort
    {
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя файла с расширением
        /// </summary>
        public string FileName { get; set; } = default!;

        /// <summary>
        /// Расшиерение файла
        /// </summary>
        public string ContentType { get; set; } = default!;

        /// <summary>
        /// Вес файла
        /// </summary>
        public long Length { get; set; }
    }
}
