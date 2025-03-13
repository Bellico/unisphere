using Unisphere.Core.Infrastructure;
using Unisphere.Explorer.Api;
using Unisphere.Explorer.Api.Endpoints;
using Unisphere.Explorer.Api.Middlewares;
using Unisphere.Explorer.Api.Services;
using Unisphere.Explorer.Application;
using Unisphere.Explorer.Infrastructure;
using Unisphere.ServiceDefaults.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .RegisterPresentationServices()
    .RegisterApplicationServices()
    .RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.MapExplorerEndpoints();

app.UseMiddleware<RequestContextLoggingMiddleware>();

app.MapGrpcService<ExplorerRpcService>();

if (app.Environment.IsDevelopment())
{
    await app.Services.ConfigureDatabaseAsync<ApplicationDbContext>();
}

await app.RunAsync();

// builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
// builder.Services.AddSwaggerGenWithAuth();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwaggerWithUi();
//     app.ApplyMigrations();
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
// app.MapHealthChecks("health", new HealthCheckOptions
// {
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
// });
// app.UseRequestContextLogging();
// app.UseSerilogRequestLogging();
// app.UseAuthentication();
// app.UseAuthorization();
// app.UseHealthChecks();
