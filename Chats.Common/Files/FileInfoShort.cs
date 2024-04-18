namespace Chats.Common.Files
{
    /// <summary>
    /// Краткая модель информации о файле
    /// </summary>
    public class FileInfoShort
    {
        /// <summary>
        /// Краткое имя файла внутри системы
        /// </summary>
        public string Link { get; set; } = default!;

        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get; set; } = default!;
    }
}
