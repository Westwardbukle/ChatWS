using AutoMapper;
using Chats.Database;
using Chats.Database.Entities;
using Chats.Domain.Interfaces;
using Chats.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MMTR.AspNet.Exceptions;
using MMTR.IoC.Interfaces;

namespace Chats.Logic.Services
{
    public class UsersInteractionChatsService : IUsersInteractionsChatsService
    {
        private readonly IBaseRepository _repository;
        private readonly IMapper _mapper;
        private readonly IScopeInfo _scopeInfo;

        public UsersInteractionChatsService(
            IBaseRepository repository,
            IMapper mapper, 
            IScopeInfo scopeInfo)
        {
            _repository = repository;
            _mapper = mapper;
            _scopeInfo = scopeInfo;
        }

        /// <inheritdoc/>
        public async Task ConnectCurrentUserToChat(Guid chatId)
        {
            var currentUserInfo = await GetCurrentUserInfo();

            var chat = await _repository.GetOneAsync<ChatEntity>(p => p.Id == chatId);
            if (chat is null)
                throw new BadRequestException("Чата с таким идентификатором не существует");

            var userAlreadyConnected = await _repository
                .FindByCondition<UserChatEntity>(p => p.ChatId == chatId && p.UserId == currentUserInfo.UserId)
                .AnyAsync();

            if (userAlreadyConnected)
                return;

            var userChat = new UserChatEntity
            {
                ChatId = chatId,
                UserId = currentUserInfo.UserId,
            };

            await _repository.CreateAsync(userChat);
        }

        /// <inheritdoc/>
        public async Task DisconnectCurrentUserToChat(Guid chatId)
        {
            var userConnected = await _repository
                .FindByCondition<UserChatEntity>(p => p.ChatId == chatId && p.UserId == _scopeInfo.UserId)
                .AnyAsync();

            if (!userConnected)
                return;

            await _repository.DeleteByCondition<UserChatEntity>(u => u.ChatId == chatId && u.UserId == _scopeInfo.UserId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserShortInfo>> GetChatMembers(Guid chatId)
        {
            var query =
                from chatUser in _repository
                    .FindByCondition<UserChatEntity>(p => p.ChatId == chatId)
                join user in _repository
                    .FindByCondition<UserInfoEntity>(p => true)
                    on chatUser.UserId equals user.UserId
                select new UserShortInfo
                {
                    UserId = user.UserId,
                    Name = user.Name
                };

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task SubscribeCurrentUserToChat(Guid chatId)
        {
            var currentUserInfo = await GetCurrentUserInfo();

            var chat = await _repository.GetOneAsync<ChatEntity>(p => p.Id == chatId);
            if (chat is null)
                throw new BadRequestException("Чата с таким идентификатором не существует");

            var userAlreadyConnected = await _repository
                .FindByCondition<SubscribeEntity>(p => p.ChatId == chatId && p.UserId == currentUserInfo.UserId)
                .AnyAsync();

            if (userAlreadyConnected)
                throw new BadRequestException("Текущий пользователь уже подписан на данный чат");

            var userChat = new SubscribeEntity
            {
                ChatId = chatId,
                UserId = currentUserInfo.UserId,
            };

            await _repository.CreateAsync(userChat);
        }

        /// <inheritdoc/>
        public async Task UnsubscribeCurrentUserToChat(Guid chatId)
        {
            var userConnected = await _repository
                .FindByCondition<SubscribeEntity>(p => p.ChatId == chatId && p.UserId == _scopeInfo.UserId)
                .AnyAsync();

            if (!userConnected)
                throw new BadRequestException("Пользователь не подписан на данный чат");

            await _repository.DeleteByCondition<SubscribeEntity>(u => u.ChatId == chatId && u.UserId == _scopeInfo.UserId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ChatShortInfo>> GetCurrentUserSubscriptions()
        {
            var query =
               from subscribe in _repository
                   .FindByCondition<SubscribeEntity>(p => p.UserId == _scopeInfo.UserId)
               join chat in _repository
                   .FindByCondition<ChatEntity>(p => true)
                   on subscribe.ChatId equals chat.Id
               select chat;

            var chats = await query.ToListAsync();

            return _mapper.Map<IEnumerable<ChatShortInfo>>(chats);
        }

        /// <inheritdoc/>
        public async Task<bool> CheckCurrentUserSubscription(Guid chatId)
        {
            return await _repository
                .FindByCondition<SubscribeEntity>(p => p.ChatId == chatId && p.UserId == _scopeInfo.UserId)
                .AnyAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserShortInfo>> GetChatSubscribers(Guid chatId)
        {
            var query =
               from chatUser in _repository
                   .FindByCondition<SubscribeEntity>(p => p.ChatId == chatId)
               join user in _repository
                   .FindByCondition<UserInfoEntity>(p => true)
                   on chatUser.UserId equals user.UserId
               select new UserShortInfo
               {
                   UserId = user.UserId,
                   Name = user.Name
               };

            return await query.ToListAsync();
        }

        public async Task<UserShortInfo> GetCurrentUserInfo()
        {
            var userInfoEntity = await _repository
                .GetOneAsync<UserInfoEntity>(p => p.UserId == _scopeInfo.UserId);

            if (userInfoEntity is null)
            {
                userInfoEntity = await _repository.CreateAsync(new UserInfoEntity
                {
                    UserId = _scopeInfo.UserId,
                    Name = _scopeInfo.UserFio,
                });
            }

            return _mapper.Map<UserShortInfo>(userInfoEntity);
        }
    }
}