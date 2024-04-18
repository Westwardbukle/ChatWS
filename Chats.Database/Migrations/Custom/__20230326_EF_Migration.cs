using System.Data.Common;
using Chats.Database.Context;
using Microsoft.EntityFrameworkCore;
using MMTR.CustomMigrations.Attributes;
using MMTR.CustomMigrations.Enums;
using MMTR.CustomMigrations.Migrators;

namespace Chats.Database.Migrations.Custom;

[CustomMigration(CustomMigrationStrategy.Anyway)]
public class __20230326_EF_Migration : CustomMigration<CommonContext>
{
    private readonly CommonContext _context;

    public __20230326_EF_Migration(CommonContext context)
    {
        _context = context;
    }

    protected override async Task Execute(DbConnection connection, CancellationToken token)
    {
        await _context.Database.MigrateAsync(token);
    }

    protected override Task<string> GetCurrentHash()
    {
        return Task.FromResult(string.Empty);
    }
}