using Chats.Common.ApiClients.Interfaces;
using Chats.Common.Models;
using Chats.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using MMTR.AspNet.Exceptions;
using MMTR.Authentication.ApiClients.Interceptors;
using MMTR.Http.Facade.Interfaces.ApiClient;
using MMTR.IoC.Interfaces;

namespace Chats.Logic.Hubs;

[Authorize]
public class NotifyHub : Hub
{
    private readonly IUsersInteractionsChatsService _usersInteractionsChatsService;
    private readonly IScopeInfoSetter _scopeInfoSetter;

    public NotifyHub(IUsersInteractionsChatsService usersInteractionsChatsService, IScopeInfoSetter scopeInfoSetter)
    {
        _usersInteractionsChatsService = usersInteractionsChatsService;
        _scopeInfoSetter = scopeInfoSetter;
    }


    public override async Task OnConnectedAsync()
    {
        var chatIdString = GetQueryParam("ChatId");

        if (!Guid.TryParse(chatIdString, out var chatId))
            throw new BadRequestException("Некорректный идентификатор чата");

        await Groups.AddToGroupAsync(Context.ConnectionId, chatIdString);

        var userInfo = await GetUserInfo();

        _scopeInfoSetter.SetUserInfo(userInfo.Id, userInfo.Fio, "ru");

        await _usersInteractionsChatsService.ConnectCurrentUserToChat(chatId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var chatIdString = GetQueryParam("ChatId");

        if (!Guid.TryParse(chatIdString, out var chatId))
            throw new BadRequestException("Некорректный идентификатор чата");

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatIdString);

        var userInfo = await GetUserInfo();

        _scopeInfoSetter.SetUserInfo(userInfo.Id, userInfo.Fio, "ru");

        await _usersInteractionsChatsService.DisconnectCurrentUserToChat(chatId);

        await base.OnDisconnectedAsync(exception);
    }

    private string GetQueryParam(string key)
    {
        var context = Context.GetHttpContext();

        var test = context?.Request.Query.TryGetValue(key, out var chatId) ?? false;

        if (!test) throw new BadRequestException("Не указан идентификатор чата");

        return chatId.First();
    }

    private async Task<UserInfo> GetUserInfo()
    {
        return await Context.GetHttpContext()!
            .RequestServices
            .GetRequiredService<IApiClientFactory<IAdministrationApiClient>>()
            .ImplementWithCurrentUser().GetUserIdentityInfo();
    }
}