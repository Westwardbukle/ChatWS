using System.Reflection;
using Chats.Application.Extensions;
using Chats.Database.Context;
using Chats.Logic.Hubs;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MMTR.AspNet;
using MMTR.AspNet.Extensions;
using MMTR.Authentication;
using MMTR.Configuration;
using MMTR.CustomMigrations;
using MMTR.CustomMigrations.Interfaces;
using MMTR.Http.Facade;
using MMTR.IoC;
using MMTR.Logging;
using MMTR.Monitoring;
using MMTR.Swagger;
using Npgsql;
using Prometheus.DotNetRuntime;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.ConfigureVault();

var apiTitle = $"{builder.Configuration.GetValue<string>("Application:AppTitle")} - Chats API";
builder.Services.AddScopeInfo();
builder.Services.AddMetrics();
builder.Services.AddBapLogger();
builder.Services.AutoRegistryOptions(builder.Configuration);
builder.Services.AutoRegistryApiClients();
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddCustomHealthChecks();

builder.Services.RegisterServices();
builder.Services.ConfigureInternalAuthentication(builder.Configuration);

builder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInternalCors(builder.Configuration);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var connectionStringSelector = () =>
{
    var result = builder.Configuration.GetValue<string>("CHATS_DB") ?? string.Empty;
    if (string.IsNullOrEmpty(result))
    {
        result = builder.Configuration.GetValue<string>("DEFAULT_DB") ?? string.Empty;
    }

    if (string.IsNullOrEmpty(result))
    {
        result = builder.Configuration.GetConnectionString("Chats") ?? string.Empty;
    }

    if (string.IsNullOrEmpty(result))
    {
        result = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    return result;
};

builder.Services.AddDbContext<CommonContext>(options =>
{
    options.UseNpgsql(connectionStringSelector());
});

builder.Services.AddCustomMigrations<CommonContext>(() => new NpgsqlConnection(connectionStringSelector()));

builder.Services.AddSwaggerWithKeycloak(builder.Configuration, options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Chats.Domain.xml"));
    options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = apiTitle });
    options.DescribeAllParametersInCamelCase();
});

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
app.UseSwaggerUIWithKeycloak(options =>
{
    options.OAuthAppName(apiTitle);
});

app.UseCors(EnvironmentConstants.CorsKey);
app.UseRequestLogging();
app.UseMetrics();
app.UseExceptionMiddleware();

app.UseInternalAuthentication();
app.MapControllers();

app.MapHub<NotifyHub>("/chat/notify", options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
});
app.MapCustomHealthCheck("/api/healthcheck");

if (EnvironmentConstants.IsMigrationMode())
{
    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<IAutoMigrator>().MigrateAsync(CancellationToken.None);
}

if (EnvironmentConstants.IsApplicationMode())
{
    using var collector = DotNetRuntimeStatsBuilder
        .Default()
        .StartCollecting();
    app.Run();
}