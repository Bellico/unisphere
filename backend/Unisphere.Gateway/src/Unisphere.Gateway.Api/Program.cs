using Microsoft.AspNetCore.RateLimiting;
using Unisphere.Gateway.Api;
using Unisphere.Gateway.Api.Middlewares;
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

builder.Services.AddExceptionHandler<GrpcExceptionHandler>();

builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

app
    .MapDefaultEndpoints()
    .MapExplorerEndpoints();

app.UseExceptionHandler();

app.UseRateLimiter();

app.MapReverseProxy();

await app.RunAsync();
