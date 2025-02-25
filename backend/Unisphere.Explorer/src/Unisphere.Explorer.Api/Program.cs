using Unisphere.Explorer.Api;
using Unisphere.Explorer.Api.Endpoints;
using Unisphere.Explorer.Api.Middlewares;
using Unisphere.Explorer.Api.RpcServices;
using Unisphere.Explorer.Application;
using Unisphere.Explorer.Infrastructure;
using Unisphere.ServiceDefaults.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .RegisterPresentationServices()
    .RegisterApplicationServices()
    .RegisterInfrastructureServices();

var app = builder.Build();

app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.MapExplorerEndpoints();

app.UseMiddleware<RequestContextLoggingMiddleware>();

app.MapGrpcService<ExplorerRpcService>();

await app.RunAsync();

//builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

//builder.Services.AddSwaggerGenWithAuth();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwaggerWithUi();

//    app.ApplyMigrations();
//}

//app.MapHealthChecks("health", new HealthCheckOptions
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

//app.UseRequestContextLogging();

//app.UseSerilogRequestLogging();

//app.UseAuthentication();

//app.UseAuthorization();
