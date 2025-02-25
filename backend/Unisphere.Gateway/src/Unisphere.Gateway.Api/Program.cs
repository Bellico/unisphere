using Microsoft.AspNetCore.RateLimiting;
using Shared.Presentation;
using Unisphere.Gateway.Api;
using Unisphere.ServiceDefaults.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddReverseProxy()
               .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
               .AddServiceDiscoveryDestinationResolver();

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("limited", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

builder.Services.AddGrpcClients();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

app.UseExceptionHandler();

app
    .MapDefaultEndpoints()
    .MapExplorerEndpoints();

app.UseRateLimiter();

app.MapReverseProxy();

await app.RunAsync();
