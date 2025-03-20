using OpenIddict.Abstractions;
using Unisphere.Core.Common.Constants;
using Unisphere.Gateway.Api.Database;

namespace Unisphere.Gateway.Api.Services
{
    internal sealed class ApplicationManagerService(IServiceProvider serviceProvider) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("service-worker", cancellationToken) is null)
            {
                await manager.CreateAsync(
                    new OpenIddictApplicationDescriptor
                    {
                        ClientId = "service-worker",
                        ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                        ClientType = OpenIddictConstants.ClientTypes.Confidential,
                        DisplayName = "Service Key",
                        Permissions =
                        {
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                            OpenIddictConstants.Permissions.ResponseTypes.Code,

                            OpenIddictConstants.Permissions.Prefixes.Scope + UnisphereConstants.Scopes.ExplorerApi,
                        },
                    },
                    cancellationToken);
            }

            if (await manager.FindByClientIdAsync("service-auth", cancellationToken) is null)
            {
                await manager.CreateAsync(
                    new OpenIddictApplicationDescriptor
                    {
                        ClientId = "service-auth",
                        ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                        ClientType = OpenIddictConstants.ClientTypes.Confidential,
                        DisplayName = "Service Auth",
                        RedirectUris = { new Uri("https://localhost:7244/callback") },
                        Permissions =
                        {
                            OpenIddictConstants.Permissions.Endpoints.Authorization,
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                            OpenIddictConstants.Permissions.ResponseTypes.Code,
                            OpenIddictConstants.Permissions.Scopes.Profile,
                            OpenIddictConstants.Permissions.Scopes.Email,

                            OpenIddictConstants.Permissions.Prefixes.Scope + UnisphereConstants.Scopes.ExplorerApi,
                        },
                        Requirements = { OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange },
                    },
                    cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
