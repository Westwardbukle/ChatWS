using System.Net;
using Chats.Common.ApiClients.Interfaces;
using Chats.Common.Authentication;
using Chats.Common.Extentions;
using Chats.Common.Filters;
using Chats.Common.Models;
using Chats.Common.Options;
using Chats.Common.Paging;
using Microsoft.Extensions.Options;
using MMTR.AspNet.Exceptions;
using MMTR.Http.Facade.ApiClient;
using MMTR.Http.Facade.Interfaces.ApiClient;

namespace Chats.Common.ApiClients
{
    public static class AdministrationApiEndpoints
    {
        public const string Settings = "api/settings";
        public const string Account = "api/account";
        public const string Client = "api/client";
        public const string Role = "api/role";
        public const string User = "api/user";
        public const string RoleDataCondition = "api/role-data-condition";
        public const string MobileApp = "api/mobile";
        public const string Brandbook = "api/brandbook";
        public const string BusinessIntelligence = "api/bi";
    }

    internal class AdministrationApiClient : ApiClientBase<AdministrationApiClient>, IAdministrationApiClient
    {
        private readonly IOptions<UriOptions> _options;

        public AdministrationApiClient(IOptions<UriOptions> options, IApiClientContext<AdministrationApiClient> context) : base(context)
        {
            _options = options;
        }

        protected override Uri BaseAddress => new(_options.Value.Administration);

        /// <summary>
        /// Получить информацию о текущем пользователе
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserIdentityInfo(CancellationToken token = default)
        {
            var result = await MakeRequestAndSendAsync<UserInfo>(
                HttpMethod.Get,
                "/api/identity",
                token
            );

            if (result.IsSuccessStatusCode is false || result.Response is null)
            {
                if (result.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new ForbiddenException("Доступ заблокирован");
                }

                throw new BadRequestException("Пользователь не найден");
            }

            return result.Response;
        }
    
        public async Task<Page<Role>?> GetRolesByUserId(Guid userId, CancellationToken token = default)
        {
            var result = await MakeRequestAndSendAsync<Page<Role>>(
                HttpMethod.Get,
                $"{AdministrationApiEndpoints.User}/{userId}/role",
                token);
            return result.Response;
        }

        public async Task<T?> GetUsersByRoleId<T>(Guid roleId, UserGridFilter? filter = null,
            CancellationToken token = default)
        {
            var parameters = filter.ToQueryString();
            var result = await MakeRequestAndSendAsync<T?>(
                HttpMethod.Get,
                $"{AdministrationApiEndpoints.Role}/{roleId}/user?{parameters}",
                token);

            return result.Response;
        }

        public async Task<UserInfo?> GetUserInfoAsync(Guid userId, CancellationToken token = default)
        {
            var result = await MakeRequestAndSendAsync<UserInfo>(
                HttpMethod.Get,
                $"{AdministrationApiEndpoints.User}/{userId}/info",
                token);

            return result.Response;
        }

        public async Task<Page<UnoGridDataRow>?> GetUsersWithProperties(UserGridFilter? filter = null,
            CancellationToken token = default)
        {
            var parameters = filter.ToQueryString();

            var result = await MakeRequestAndSendAsync<Page<UnoGridDataRow>>(
                HttpMethod.Get,
                $"{AdministrationApiEndpoints.User}?{parameters}",
                token);

            return result.Response;
        }

        public async Task<ObjectDataShortView?> GetClientLinkedUser(Guid clientId, CancellationToken token = default)
        {
            var result = await MakeRequestAndSendAsync<ObjectDataShortView>(
                HttpMethod.Get,
                $"{AdministrationApiEndpoints.Client}/{clientId}/user",
                token);

            return result.Response;
        }

        public async Task<UserActivityStatus?> GetUserActivity(Guid userId, CancellationToken token = default)
        {
            var result = await MakeRequestAndSendAsync<UserActivityStatus>(
                HttpMethod.Get,
                $"{AdministrationApiEndpoints.User}/{userId}/activity-status",
                token);

            return result.Response;
        }
    }
}