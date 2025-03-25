using Unisphere.Core.Common.Constants;
using Unisphere.Core.Infrastructure;
using Unisphere.Core.Presentation.Extensions;
using Unisphere.Identity.Application;
using Unisphere.Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddUnisphereCore()
    .AddAuthenticationServices()
    .AddAuthorizationServices(UnisphereConstants.PoliciesNames.IdentityPolicy, UnisphereConstants.Scopes.IdentityApi);

builder.Services
    .RegisterApplicationServices()
    .RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseUnisphereCore();

app
    .MapGroup("/")
    .RequireAuthorization(UnisphereConstants.PoliciesNames.IdentityPolicy)
    .MapEndpoints();

if (app.Environment.IsDevelopment())
{
    await app.Services.ConfigureDatabaseAsync<UnisphereIdentityDbContext>();
}

await app.RunAsync();
