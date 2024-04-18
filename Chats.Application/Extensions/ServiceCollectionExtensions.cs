using Chats.Common.Helpers;
using Chats.Database;
using Chats.Database.Repository;
using Chats.Domain.Interfaces;
using Chats.Logic.Services;
using MMTR.Authentication.UserIdentifier;
using MMTR.Configuration.Vault;

namespace Chats.Application.Extensions
{
    /// <summary>
    /// Расширение для регистрации сервисов приложения
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public static void ConfigureVault(this IConfigurationBuilder configuration)
        {
            configuration
                .AddEnvironmentVariables()
                .AddVault()
                .Build();
        }

        /// <summary>
        /// Регистрация сервисов
        /// </summary>
        /// <param name="services">Сервисы</param>
        public static void RegisterServices(this IServiceCollection services)
        {
            // Сервисы
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUsersInteractionsChatsService, UsersInteractionChatsService>();
            services.AddScoped<IUILinkGenerator, UILinkGenerator>();
            services.AddScoped<IUserIdentifier, InternalUserIdentifier>();

            // Репозитории
            services.AddScoped<IBaseRepository, BaseRepository>();
        }
    }
}