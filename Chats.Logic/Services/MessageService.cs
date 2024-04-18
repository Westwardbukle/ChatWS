using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Chats.Common;
using Chats.Common.Files;
using Chats.Common.Filters;
using Chats.Common.Helpers;
using Chats.Common.Models;
using Chats.Common.Options;
using Chats.Common.Paging;
using Chats.Database;
using Chats.Database.Entities;
using Chats.Domain.Interfaces;
using Chats.Domain.Models;
using Chats.Domain.Models.Requests;
using Chats.Logic.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MMTR.AspNet.Exceptions;
using MMTR.Authentication.ApiClients.Interceptors;
using MMTR.BAP.Common.ApiClients.Interfaces;
using MMTR.BAP.Common.JsonConverters;
using MMTR.BAP.Common.Models.Grid;
using MMTR.Http.Facade.Interfaces.ApiClient;
using MMTR.IoC.Interfaces;
using IFileStorageApiClient = Chats.Common.ApiClients.Interfaces.IFileStorageApiClient;
using INotificationApiClient = Chats.Common.ApiClients.Interfaces.INotificationApiClient;

namespace Chats.Logic.Services
{
    public class MessageService : IMessageService
    {
        private readonly IBaseRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotifyHub> _notifyHubContext;
        private readonly IUsersInteractionsChatsService _usersInteractions;
        private readonly IApiClientFactory<IFileStorageApiClient> _fileStorageApiClientFactory;
        private readonly IApiClientFactory<INotificationApiClient> _notificationApiClientFactory;
        private readonly IUILinkGenerator _uiILinkGenerator;
        private readonly IScopeInfo _scopeInfo;
        private readonly Lazy<HashSet<string>> _validExtensions;
        private string _validStringExtensions;
        private readonly IApiClientFactory<IUnoApiClient> _unoApiClient;

        public MessageService(
            IBaseRepository repository,
            IMapper mapper,
            IHubContext<NotifyHub> notifyHubContext,
            IUsersInteractionsChatsService usersInteractions,
            IApiClientFactory<IFileStorageApiClient> fileStorageApiClientFactory,
            IApiClientFactory<INotificationApiClient> notificationApiClientFactory,
            IUILinkGenerator uiILinkGenerator,
            IScopeInfo scopeInfo,
            IOptions<MinioOptions> options,
            IApiClientFactory<IUnoApiClient> unoApiClient)
        {
            _repository = repository;
            _mapper = mapper;
            _notifyHubContext = notifyHubContext;
            _usersInteractions = usersInteractions;
            _fileStorageApiClientFactory = fileStorageApiClientFactory;
            _notificationApiClientFactory = notificationApiClientFactory;
            _uiILinkGenerator = uiILinkGenerator;
            _scopeInfo = scopeInfo;
            _unoApiClient = unoApiClient;
            _validExtensions =
                new Lazy<HashSet<string>>(() =>
                {
                    _validStringExtensions = options.Value.DocumentValidExtensions;
                    if (string.IsNullOrEmpty(_validStringExtensions))
                    {
                        _validStringExtensions = ".doc,.docx,.xls,.xlsx,.pdf,.jpg,.jpeg,.png,.odt,.ods,.apk,.msg,.mp4,.avi,.wmv,.mov,.flv,.mkv,.wav,.mp3,.m4a";
                    }
                    return _validStringExtensions.Split(",").ToHashSet();
                });
        }

        /// <inheritdoc/>
        public async Task<MessageShortInfo> CreateMessage(CreateMessageRequest request)
        {
            if (request.Text is null && request.PinnedFiles is null)
                throw new BadRequestException(
                    "Сообщение должно содержать текст и/или прикрепленные файлы");

            var currentUserInfo = await _usersInteractions.GetCurrentUserInfo();

            var chat = await _repository.GetOneAsync<ChatEntity>(p => p.Id == request.ChatId);
            if (chat is null)
                throw new BadRequestException("Чата с таким идентификатором не существует");

            var fileViewModels = new List<FileViewModel>();

            if (request.PinnedFiles is not null)
            {
                if (request.PinnedFiles.Select(pinnedFile => Path.GetExtension(pinnedFile.FileName)
                        .ToLowerInvariant())
                    .Any(extension => !string.IsNullOrEmpty(extension) && !_validExtensions.Value.Contains(extension)))
                {
                    throw new BadRequestException(
                        $"Файлы не прошли валидацию, разрешенные форматы файлов:{_validStringExtensions}");
                }

                var fileStorageClient = _fileStorageApiClientFactory.ImplementWithSystemUser();

                foreach (var file in request.PinnedFiles)
                {
                    var fileResult = await fileStorageClient.UploadFileAsync(file);

                    if (fileResult != null) fileViewModels.Add(fileResult);
                    else
                        throw new BadRequestException(
                            "Не удалось сохранить файлы отправленные в письме. Обратитесь к адмнинистратору");
                }
            }

            if (request.MentionedUsersIds != null && request.MentionedUsersIds.Any())
            {
                var notificationClient = _notificationApiClientFactory.ImplementWithSystemUser();

                var collection = new List<PushCollectionNotificationRequest>();

                foreach (var mentionedUserId in request.MentionedUsersIds)
                {
                    var notificationRequest = new PushCollectionNotificationRequest
                    {
                        UserId = mentionedUserId,
                        MessagesForRecipients = new PushNotificationRequest
                        {
                            Message = request.Text ?? string.Empty,
                            Theme = "Вас упомянули в беседе",
                            RedirectUrl = await GetChatUrl(chat)
                        }
                    };
                    collection.Add(notificationRequest);
                }

                notificationClient.SendPushCollectionUsers(collection);
            }

            var message = new MessageEntity
            {
                ChatId = request.ChatId,
                MentionedUsersIds = request.MentionedUsersIds,
                UserId = currentUserInfo.UserId,
                Text = request.Text,
                Type = request.Type,
                PinnedFiles = fileViewModels,
                ReadBy = new[] { currentUserInfo.UserId }
            };

            var createdMessage = await _repository.CreateAsync(message);

            await ReadMessagesUpToSpecificMessage(request.ChatId, createdMessage.Id);

            var result = _mapper.Map<MessageShortInfo>(createdMessage);

            result.ReadByCurrentUser = true;
            result.User = currentUserInfo;

            await _notifyHubContext.Clients.Group(message.ChatId.ToString()).SendAsync("Message", result);

            await SendPushNotifications(message, chat);

            return result;
        }

        /// <inheritdoc/>
        public async Task<MessageInChatInfo> GetPageMessagesFromChat(Guid chatId, MessageFilter filter)
        {
            if (!_repository.FindByCondition<ChatEntity>(p => p.Id == chatId).Any())
                throw new BadRequestException($"Чат с идентификатором {chatId} не найден");

            var currentMessage = new MessageEntity();

            if (filter.MessageId is not null)
                currentMessage = await _repository
                    .FindByCondition<MessageEntity>(p => p.ChatId == chatId && p.Id == filter.MessageId)
                    .OrderBy(p => p.DateCreated).FirstOrDefaultAsync();
            else
                currentMessage = await _repository
                    .FindByCondition<MessageEntity>(p =>
                        p.ChatId == chatId && p.ReadBy.Contains(_scopeInfo.UserId))
                    .OrderByDescending(p => p.DateCreated).FirstOrDefaultAsync();

            var countUnread = _repository
                    .FindByCondition<MessageEntity>(p => p.ChatId == chatId)
                    .Count(p => p.ChatId == chatId && !p.ReadBy.Contains(_scopeInfo.UserId));

            if (currentMessage is null)
            {
                var query =
                    from message in _repository
                        .FindByCondition<MessageEntity>(p => p.ChatId == chatId)
                        .OrderBy(p => p.DateCreated)
                        .Take(filter.PageSize)
                    join user in _repository.FindByCondition<UserInfoEntity>(p => true)
                        on message.UserId equals user.UserId
                    select new MessageShortInfo
                    {
                        Id = message.Id,
                        ChatId = message.ChatId,
                        User = new UserShortInfo
                        {
                            UserId = user.UserId,
                            Name = user.Name
                        },
                        Text = message.Text,
                        Type = message.Type,
                        PinnedFiles = message.PinnedFiles,
                        DateCreated = message.DateCreated,
                        ReadByCurrentUser = message.ReadBy.Contains(_scopeInfo.UserId)
                    };

                var messageShortInfos = await GetMiniatureLinks(await query.ToListAsync());

                return new MessageInChatInfo
                {
                    Messages = messageShortInfos,
                    CountUnread = countUnread
                };
            }

            if (filter.Up)
            {
                var query =
                    from message in _repository
                        .FindByCondition<MessageEntity>(p => p.ChatId == chatId)
                        .OrderByDescending(p => p.DateCreated)
                        .Where(p => p.DateCreated <= currentMessage.DateCreated)
                        .Take(filter.PageSize)
                    join user in _repository.FindByCondition<UserInfoEntity>(p => true)
                        on message.UserId equals user.UserId
                    select new MessageShortInfo
                    {
                        Id = message.Id,
                        ChatId = message.ChatId,
                        User = new UserShortInfo
                        {
                            UserId = user.UserId,
                            Name = user.Name
                        },
                        Text = message.Text,
                        Type = message.Type,
                        PinnedFiles = message.PinnedFiles,
                        DateCreated = message.DateCreated,
                        ReadByCurrentUser = message.ReadBy.Contains(_scopeInfo.UserId)
                    };

                var messageShortInfos = await GetMiniatureLinks(await query.ToListAsync());

                return new MessageInChatInfo
                {
                    Messages = messageShortInfos.OrderBy(x => x.DateCreated),
                    CountUnread = countUnread
                };
            }
            else
            {
                var query =
                    from message in _repository
                        .FindByCondition<MessageEntity>(p => p.ChatId == chatId)
                        .OrderBy(p => p.DateCreated)
                        .Where(p => p.DateCreated >= currentMessage.DateCreated)
                        .Take(filter.PageSize)
                    join userInfo in _repository.FindByCondition<UserInfoEntity>(p => true)
                        on message.UserId equals userInfo.UserId
                    select new MessageShortInfo
                    {
                        Id = message.Id,
                        ChatId = message.ChatId,
                        User = new UserShortInfo
                        {
                            UserId = userInfo.UserId,
                            Name = userInfo.Name
                        },
                        Text = message.Text,
                        Type = message.Type,
                        PinnedFiles = message.PinnedFiles,
                        DateCreated = message.DateCreated,
                        ReadByCurrentUser = message.ReadBy.Contains(_scopeInfo.UserId)
                    };

                var messageShortInfos = await GetMiniatureLinks(await query.ToListAsync());

                return new MessageInChatInfo
                {
                    Messages = messageShortInfos,
                    CountUnread = countUnread
                };
            }
        }

        /// <inheritdoc/>
        public async Task<MessageShortInfo> DeleteMessage(Guid messageId)
        {
            var deletedMessage = await _repository.DeleteAsync<MessageEntity>(messageId);
            if (deletedMessage is null)
                throw new NotFoundException($"Сообщение с идентификатором {messageId} не найдено.");

            return _mapper.Map<MessageShortInfo>(deletedMessage);
        }

        /// <inheritdoc/>
        public async Task<int> ReadMessagesUpToSpecificMessage(Guid chatId, Guid? messageId)
        {
            var unreadMessages = new List<MessageEntity>();

            if (messageId is null)
            {
                unreadMessages = await _repository
                    .FindByCondition<MessageEntity>(p => p.ChatId == chatId && !p.ReadBy.Contains(_scopeInfo.UserId))
                    .ToListAsync();

                if (unreadMessages.Any())
                {

                    foreach (var message in unreadMessages)
                    {
                        message.ReadBy = message.ReadBy.Append(_scopeInfo.UserId).ToArray();
                    }

                    await _repository.UpdateRange(unreadMessages);
                }

                return 0;
            }
            else
            {
                var messageEntity = await _repository.GetOneAsync<MessageEntity>(p => p.Id == messageId);
                if (messageEntity is null)
                    throw new NotFoundException($"Сообщение с идентификатором {messageId} не найдено");

                unreadMessages = await _repository
                    .FindByCondition<MessageEntity>(p => p.ReadBy != null && p.ChatId == chatId &&
                        p.DateCreated <= messageEntity.DateCreated && !p.ReadBy.Contains(_scopeInfo.UserId))
                    .ToListAsync();

                if (!unreadMessages.Any())
                    return 0;

                foreach (var message in unreadMessages)
                {
                    message.ReadBy = message.ReadBy.Append(_scopeInfo.UserId).ToArray();
                }

                await _repository.UpdateRange(unreadMessages);

                return _repository
                    .FindByCondition<MessageEntity>(p => p.ChatId == chatId && !p.ReadBy.Contains(_scopeInfo.UserId))
                    .Count();
            }
        }

        #region Private


        /// <summary>
        /// Отправить пуш уведомления по подписке
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="chat">Чат</param>
        /// <returns></returns>
        private async Task SendPushNotifications(MessageEntity message, ChatEntity chat)
        {
            // Выбираем подписчиков, кроме тех, кто находится в чате
            var chatUsers = await _repository
                .FindByCondition<UserChatEntity>(p => p.ChatId == message.ChatId)
                .Select(x => x.UserId)
                .ToListAsync();

            var subscribers = await _repository
                .FindByCondition<SubscribeEntity>(p => p.ChatId == message.ChatId && !chatUsers.Contains(p.UserId))
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();

            var request = new PushNotificationRequest
            {
                Message = message.Text,
                RedirectUrl = await GetChatUrl(chat)
            };

            var notificationClient = _notificationApiClientFactory.ImplementWithSystemUser();

            await notificationClient.SendPushCollectionUsers(subscribers.Select(x => new PushCollectionNotificationRequest
            {
                UserId = x,
                MessagesForRecipients = request
            }));
        }

        private async Task<string?> GetChatUrl(ChatEntity chat)
        {
            if (chat.PinnedClassId is not null && chat.PinnedObjectId is not null)
                return await _uiILinkGenerator.GetProcessedClass(chat.PinnedClassId.Value, chat.PinnedObjectId.Value);

            return null;
        }

        private async Task<IEnumerable<MessageShortInfo>> GetMiniatureLinks(IEnumerable<MessageShortInfo> query)
        {
            var usersIds = query.Select(p => p.User.UserId).Distinct();

            var unoApiClient = _unoApiClient.ImplementWithSystemUser();

            var unoObjectsWithProperties = await unoApiClient.GetObjectsWithPropertiesAsync<Page<UnoGridDataRow>>(
                Guid.Parse(Constants.UserClassId), new UnoGridDataRequest(), usersIds,
                new[] { Guid.Parse(Constants.MiniatureLinkPropertyDefinition) });

            if (unoObjectsWithProperties is null || !unoObjectsWithProperties.Content.Any())
            {
                return query;
            }

            foreach (var messageShort in query)
            {
                var unoGridData = unoObjectsWithProperties.Content
                    .FirstOrDefault(p => p.ObjectId == messageShort.User.UserId);

                if (unoGridData != null && unoGridData.Properties.Any())
                {
                    var fileInfo = Convert<FileInfoShort>(unoGridData.Properties.First().Value);

                    if (fileInfo?.Link is not null)
                        messageShort.User.MiniatureLink = fileInfo.Link;
                }
            }

            return query;
        }

        private T? Convert<T>(object? obj)
        {
            if (obj is null)
                return default;

            if (obj is IEnumerable<object> enumObj)
                obj = enumObj.Where(p => p != null);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
            options.Converters.Add(new DateOnlyJsonConverter());
            options.Converters.Add(new TimeOnlyJsonConverter());

            try
            {
                return JsonSerializer.SerializeToDocument(obj, options).Deserialize<T>(options);
            }
            catch (Exception ex)
            {
                if (obj.GetType() == typeof(string))
                    return JsonSerializer.Deserialize<T>(obj.ToString(), options);

                if (typeof(T) == typeof(string))
                    return (T)System.Convert.ChangeType(JsonSerializer.Serialize(obj, options), typeof(T));

                try
                {
                    var jsonObj = JsonSerializer.Serialize(obj);
                }
                catch (Exception)
                { }

                return default;
            }
        }

        #endregion
    }
}