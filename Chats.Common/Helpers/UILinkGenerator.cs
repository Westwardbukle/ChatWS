using Chats.Common.Options;
using Microsoft.Extensions.Options;

namespace Chats.Common.Helpers
{
    /// <summary>
    ///
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface IUILinkGenerator
    {
        Task<string> GetProcessedClass(Guid classId, Guid objectId);
    }

    /// <summary>
    ///
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class UILinkGenerator : IUILinkGenerator
    {
        private readonly IOptions<UriOptions> _options;

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        public UILinkGenerator(IOptions<UriOptions> options)
        {
            _options = options;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<string> GetProcessedClass(Guid classId, Guid objectId)
        {
            return Task.FromResult($"{_options.Value.Frontend}/processed/{classId}/{objectId}");
        }
    }
}