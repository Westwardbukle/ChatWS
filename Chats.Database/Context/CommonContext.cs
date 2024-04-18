using Chats.Database.Entities;
using Microsoft.EntityFrameworkCore;
using MMTR.CustomMigrations.Interfaces;

namespace Chats.Database.Context
{
    public class CommonContext : DbContext, IMigrationSpace
    {
        public DbSet<ChatEntity> Chats { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<UserChatEntity> UserChats { get; set; }

        public DbSet<SubscribeEntity> Subscriptions { get; set; }

        public DbSet<UserInfoEntity> UserInfo { get; set; }

        public CommonContext(DbContextOptions<CommonContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ChatEntity>();

            builder.Entity<MessageEntity>();

            builder.Entity<UserChatEntity>()
                .HasOne(p => p.UserInfo)
                .WithMany(p => p.UserChat)
                .HasForeignKey(p => p.UserId)
                .HasPrincipalKey(p => p.UserId);

            builder.Entity<SubscribeEntity>()
                .HasOne(p => p.UserInfo)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(p => p.UserId)
                .HasPrincipalKey(p => p.UserId);

            builder.Entity<UserInfoEntity>();
        }
    }
}