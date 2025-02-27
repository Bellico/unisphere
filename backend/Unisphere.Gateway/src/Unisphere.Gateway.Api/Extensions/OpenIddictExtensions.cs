using Microsoft.EntityFrameworkCore;
using Unisphere.Gateway.Api.Database;

namespace Unisphere.Gateway.Api.Extensions
{
    internal static class OpenIddictExtensions
    {
        public static void AddOpenIddict(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Configure the Entity Framework Core to use Microsoft SQL Server.
                options.UseSqlServer(configuration.GetConnectionString("sql-openiddict"));

                // Register the entity sets needed by OpenIddict.
                options.UseOpenIddict();
            });

            services.AddOpenIddict()
                .AddCore(
                    options =>
                    {
                        options.UseEntityFrameworkCore()
                               .UseDbContext<ApplicationDbContext>();
                    })
                .AddServer(
                    options =>
                    {
                        // options.RegisterScopes(AuthorizationController.AuthorizedScopesForServerApplications);
                        options.SetTokenEndpointUris("/connect/token");
                        options.UseAspNetCore()
                                .EnableTokenEndpointPassthrough();

                        options.AddEphemeralSigningKey()
                               .AddEphemeralEncryptionKey();

                        // Allow client applications to use the grant_type=cient_credentials flow.
                        options.AllowClientCredentialsFlow();
                    })

                // Register the OpenIddict validation components.
                .AddValidation(options =>
                {
                    // Import the configuration from the local OpenIddict server instance.
                    options.UseLocalServer();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });
        }
    }
}
