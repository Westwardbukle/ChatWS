using AutoMapper;
using Chats.Database.Entities;
using Chats.Domain.Models;

namespace Chats.Application.MapperProfiles
{
    /// <summary>
    /// Моппинг моделек
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public MapperProfile()
        {
            CreateMap<ChatShortInfo, ChatEntity>().ReverseMap();

            CreateMap<MessageShortInfo, MessageEntity>().ReverseMap();
            
            CreateMap<UserChatShortInfo, UserChatEntity>().ReverseMap();

            CreateMap<UserShortInfo, UserInfoEntity>().ReverseMap();
        }
    }
}