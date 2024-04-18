using AutoMapper;
using Chats.Common.Extentions;
using Chats.Common.Filters.Paging;
using Chats.Common.Paging;
using Chats.Database;
using Chats.Database.Entities;
using Chats.Domain.Interfaces;
using Chats.Domain.Models;
using Chats.Domain.Models.Requests;
using Microsoft.EntityFrameworkCore;
using MMTR.AspNet.Exceptions;
using MMTR.IoC.Interfaces;

namespace Chats.Logic.Services;

public class ChatService : IChatService
{
    private readonly IScopeInfo _scopeInfo;
    private readonly IMapper _mapper;
    private readonly IBaseRepository _repository;
    private readonly IUsersInteractionsChatsService _usersInteractionsChatsService;

    public ChatService(
        IScopeInfo scopeInfo,
        IBaseRepository repository,
        IMapper mapper,
        IUsersInteractionsChatsService usersInteractionsChatsService)
    {
        _scopeInfo = scopeInfo;
        _repository = repository;
        _mapper = mapper;
        _usersInteractionsChatsService = usersInteractionsChatsService;
    }

    public async Task<ChatShortInfo> CreateChat(CreateChatRequest request)
    {
        if (request.PinnedObjectId is not null)
        {
            var existed = await _repository.GetOneAsync<ChatEntity>(p =>
                p.PinnedObjectId == request.PinnedObjectId);

            if (existed is not null)
                throw new BadRequestException("Чат с таким прикрепленным объектом уже существует");
        }

        var chatEntity = await _repository.CreateAsync(new ChatEntity
        {
            ChatName = request.ChatName,
            Description = request.Description,
            PinnedClassId = request.PinnedClassId,
            PinnedObjectId = request.PinnedObjectId,
            Icon = request.Icon
        });

        var result = _mapper.Map<ChatShortInfo>(chatEntity);

        return result;
    }

    public async Task<ChatShortInfo> GetChatById(Guid chatId)
    {
        var chat = await _repository.GetOneAsync<ChatEntity>(p => p.Id == chatId);
        if (chat is null) 
            throw new NotFoundException("Чат с таким идентификатором не найден");
        
        var members = await _usersInteractionsChatsService.GetChatMembers(chatId);

        var result = _mapper.Map<ChatShortInfo>(chat);
        result.Members = _mapper.Map<List<UserShortInfo>>(members);

        return result;
    }

    public async Task<Page<ChatShortInfo>> GetCurrentUserChatsPage(BasePageFilter filter)
    {
        var chats = await _repository.FindByCondition<UserChatEntity>(p => p.UserId == _scopeInfo.UserId)
            .Select(p => p.Chat).ToListAsync();

        var result = _mapper.Map<List<ChatShortInfo>>(chats);

        return result.ToPage(filter);
    }

    public async Task<ChatShortInfo> GetPinnedChat(Guid classId, Guid objectId)
    {
        var pinnedChat = await _repository.GetOneAsync<ChatEntity>(p => p.PinnedObjectId == objectId);

        if (pinnedChat is null)
        {
            var createChat = new CreateChatRequest
            {
                ChatName = "Комментарии",
                PinnedClassId = classId,
                PinnedObjectId = objectId
            };

            var chat = await CreateChat(createChat);

            return chat;
        }

        return _mapper.Map<ChatShortInfo>(pinnedChat);
    }

    public async Task<ChatShortInfo> UpdateChat(Guid chatId, CreateChatRequest request)
    {
        var exist = _repository.FindByCondition<ChatEntity>(p => p.Id == chatId).Any();
        if (!exist) 
            throw new NotFoundException($"Чат с идентификатором {chatId} не найденю");

        var chatEntity = new ChatEntity
        {
            Id = chatId,
            ChatName = request.ChatName,
            Description = request.Description,
            Icon = request.Icon
        };

        var chat = await _repository.UpdateAsync(chatEntity);

        return _mapper.Map<ChatShortInfo>(chat);
    }

    public async Task<ChatShortInfo> DeleteChat(Guid chatId)
    {
        var deletedChat = await _repository.DeleteAsync<ChatEntity>(chatId);
        if (deletedChat is null) 
            throw new NotFoundException($"Чат с идентификатором {chatId} не найден");

        return _mapper.Map<ChatShortInfo>(deletedChat);
    }
}