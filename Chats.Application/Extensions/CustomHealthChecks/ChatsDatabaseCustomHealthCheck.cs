using Chats.Common.Options;
using Chats.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MMTR.AspNet.HealthChecks;
using MMTR.AspNet.Interfaces;

namespace Chats.Application.Extensions.CustomHealthChecks;

internal class ChatsDatabaseCustomHealthCheck : DatabaseCustomHealthCheck<ChatsDatabaseCustomHealthCheck>
{
    private readonly CommonContext _context;
    private readonly IOptions<HealthCheckOptions> _options;

    public ChatsDatabaseCustomHealthCheck(CommonContext context, IOptions<HealthCheckOptions> options,
        IExceptionCounter<ChatsDatabaseCustomHealthCheck> databaseExceptionCounter) : base(databaseExceptionCounter)
    {
        _context = context;
        _options = options;
    }

    protected override async Task SendRequest()
    {
        await _context.Chats.Select(x => x.Id).FirstOrDefaultAsync();
    }
}