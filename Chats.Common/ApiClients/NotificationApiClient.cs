using Chats.Common.ApiClients.ApiEndpoints;
using Chats.Common.ApiClients.Interfaces;
using Chats.Common.Models;
using Chats.Common.Options;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using MMTR.Http.Facade.ApiClient;
using MMTR.Http.Facade.Attributes;
using MMTR.Http.Facade.Interfaces.ApiClient;
using MMTR.Http.Facade.Models;

namespace Chats.Common.ApiClients
{
    [UsedImplicitly]
    [ApiClient("notifications")]
    internal class NotificationApiClient : ApiClientBase<NotificationApiClient>, INotificationApiClient
    {
        private readonly IOptions<UriOptions> _options;

        public NotificationApiClient(IOptions<UriOptions> options, IApiClientContext<NotificationApiClient> context)
            : base(context)
        {
            _options = options;
        }

        protected override Uri BaseAddress => new(_options.Value.Notification);

        public async Task<ApiResponse> SendPushByRole(string message, Guid roleId, string? redirectUrl = null,
            CancellationToken token = default)
        {
            var redirectUrlQuery = redirectUrl is null ? string.Empty : $"&redirectUrl={redirectUrl}";
            var request = await MakeRequestMessageAsync(HttpMethod.Post, $"{NotificationApiEndpoints.Push}/role/{roleId}?message={message}{redirectUrlQuery}", message, token);
            return await SendAsync(request, token);
        }

        public async Task<ApiResponse> SendPushByUserId(
            string message,
            Guid userId,
            string? theme = null,
            string? redirectUrl = null,
            CancellationToken token = default)
        {
            var themeQuery = theme is null ? string.Empty : $"&theme={theme}"; 
            var redirectUrlQuery = redirectUrl is null ? string.Empty : $"&redirectUrl={redirectUrl}";
            var request = await MakeRequestMessageAsync(HttpMethod.Post, $"{NotificationApiEndpoints.Push}/user/{userId}?message={message}{themeQuery}{redirectUrlQuery}", message, token);

            return await SendAsync(request, token);
        }

        public async Task<ApiResponse> SendPushCollectionUsers(
            IEnumerable<PushCollectionNotificationRequest> messages,
            CancellationToken token = default)
        {
            var request = await MakeRequestMessageAsync(HttpMethod.Post, $"{NotificationApiEndpoints.Push}/users", messages, token);

            return await SendAsync(request, token);
        }

        public async Task<ApiResponse> SendEmailNotification(
            IEnumerable<string> recipients,
            string message,
            string? subject = null,
            CancellationToken token = default)
        {
            var dataContent = new MultipartFormDataContent();
            foreach (var recipient in recipients)
                dataContent.Add(new StringContent("{ \"Email\":\"" + $"{recipient}" + "\"}"), "Recipients");

            if (!string.IsNullOrEmpty(subject))
                dataContent.Add(new StringContent(subject), "Subject");

            dataContent.Add(new StringContent($"{message}"), "Content");

            var request = await MakeRequestMessageAsync(HttpMethod.Post, NotificationApiEndpoints.Email, token);
            request.Content = dataContent;

            return await SendAsync(request, token);
        }

        public async Task<ApiResponse> SendPushNotificationAllConnectedUsers(string message, CancellationToken token = default)
        {
            var request = await MakeRequestMessageAsync(HttpMethod.Post, $"{NotificationApiEndpoints.Push}?message={message}", message, token);

            return await SendAsync(request, token);
        }
    }
}