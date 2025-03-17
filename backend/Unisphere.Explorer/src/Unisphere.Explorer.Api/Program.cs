using Unisphere.Core.Infrastructure;
using Unisphere.Core.Presentation.Extensions;
using Unisphere.Explorer.Api;
using Unisphere.Explorer.Api.Endpoints;
using Unisphere.Explorer.Api.Middlewares;
using Unisphere.Explorer.Api.Services;
using Unisphere.Explorer.Application;
using Unisphere.Explorer.Infrastructure;
using Unisphere.ServiceDefaults.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAuthenticationServices();

builder.Services.AddAuthorization();

builder.Services
    .RegisterPresentationServices()
    .RegisterApplicationServices()
    .RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapExplorerEndpoints(); // Not necessary, just an example

app.UseMiddleware<RequestContextLoggingMiddleware>();

app.MapGrpcService<ExplorerRpcService>();

if (app.Environment.IsDevelopment())
{
    await app.Services.ConfigureDatabaseAsync<ApplicationDbContext>();
}

await app.RunAsync();

// builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
// app.UseRequestContextLogging();
// app.UseSerilogRequestLogging();
