using Microsoft.AspNetCore.RateLimiting;
using Unisphere.Core.Infrastructure;
using Unisphere.Core.Presentation;
using Unisphere.Gateway.Api;
using Unisphere.Gateway.Api.Database;
using Unisphere.Gateway.Api.Extensions;
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

builder.Services.AddOpenIddictAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddGrpcClients();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "Cors",
        policy =>
        {
            policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        });
});

WebApplication app = builder.Build();

app.UseExceptionHandler();

app.UseHsts();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app
    .MapDefaultEndpoints()
    .MapAuthorizationEndpoints()
    .MapExplorerEndpoints();

app.UseRateLimiter();

app.MapReverseProxy();

if (app.Environment.IsDevelopment())
{
    await app.Services.ConfigureDatabaseAsync<ApplicationDbContext>();
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

await app.RunAsync();
