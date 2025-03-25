using Unisphere.Core.Common.Constants;
using Unisphere.Core.Infrastructure;
using Unisphere.Core.Presentation.Extensions;
using Unisphere.Explorer.Api.Services;
using Unisphere.Explorer.Application;
using Unisphere.Explorer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddUnisphereCore()
    .AddAuthenticationServices()
    .AddAuthorizationServices(UnisphereConstants.PoliciesNames.ExplorerPolicy, UnisphereConstants.Scopes.ExplorerApi);

builder.Services
    .RegisterApplicationServices()
    .RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseUnisphereCore();

app
    .MapGroup("/")
    .RequireAuthorization(UnisphereConstants.PoliciesNames.ExplorerPolicy)
    .MapEndpoints();

app.MapGrpcService<ExplorerRpcService>();

if (app.Environment.IsDevelopment())
{
    await app.Services.ConfigureDatabaseAsync<ExplorerDbContext>();
}

await app.RunAsync();

// builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
// app.UseRequestContextLogging();
// app.UseSerilogRequestLogging();
