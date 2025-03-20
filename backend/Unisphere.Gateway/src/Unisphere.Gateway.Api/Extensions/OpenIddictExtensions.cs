using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Unisphere.Core.Common.Constants;
using Unisphere.Gateway.Api.Database;
using Unisphere.Gateway.Api.Services;

namespace Unisphere.Gateway.Api.Extensions
{
    internal static class OpenIddictExtensions
    {
        public static void AddAuthenticationGateway(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            }).AddCookie();

            services.AddHostedService<ApplicationManagerService>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("db-gateway"));
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
                        options
                            .SetTokenEndpointUris("/connect/token")
                            .SetAuthorizationEndpointUris("/connect/authorization");

                        options.UseAspNetCore()
                               .EnableTokenEndpointPassthrough()
                               .EnableAuthorizationEndpointPassthrough();

                        options.RegisterScopes([
                            OpenIddictConstants.Scopes.Profile,
                            OpenIddictConstants.Scopes.Email,
                            .. UnisphereConstants.Scopes.AuthorizedScopes]);

                        options.AddDevelopmentEncryptionCertificate()
                               .AddDevelopmentSigningCertificate();

                        options.DisableAccessTokenEncryption();

                        options
                            .AllowClientCredentialsFlow()
                            .AllowAuthorizationCodeFlow()
                            .AllowRefreshTokenFlow()
                            .RequireProofKeyForCodeExchange();
                    })
                .AddValidation(options =>
                {
                    options.UseLocalServer();

                    options.UseAspNetCore();
                })
                .AddClient(options =>
                {
                    options.AllowAuthorizationCodeFlow();

                    options.UseAspNetCore()
                           .EnableRedirectionEndpointPassthrough();

                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate();

                    options.UseWebProviders()
                            .AddSpotify(options =>
                            {
                                options
                                    .SetClientId("0e45085f6f1c45afb1c04e6b7af61061")
                                    .SetRedirectUri(new Uri("https://localhost:7244/callback-spotify"));
                            });
                });
        }
    }
}
