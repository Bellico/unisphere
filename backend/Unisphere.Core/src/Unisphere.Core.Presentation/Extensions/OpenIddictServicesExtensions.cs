using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Unisphere.Core.Common.Constants;

namespace Unisphere.Core.Presentation.Extensions
{
    public static class OpenIddictServicesExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            services.AddOpenIddict()
                .AddValidation(options =>
                {
                    // OpenID Connect discovery
                    options.SetIssuer(new Uri("https://localhost:7244/"));

                    // Register the System.Net.Http integration.
                    options.UseSystemNetHttp();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });

            return services;
        }

        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services, string policyName, string scopeName)
        {
            return services.AddAuthorization(
                options =>
                {
                    options.AddPolicy(
                         UnisphereConstants.PoliciesNames.ExplorerPolicy, policy =>
                         policy
                            .RequireAuthenticatedUser()
                            .RequireClaim("sub")
                            .RequireAssertion(x => x.User.HasScope(UnisphereConstants.Scopes.ExplorerApi)));
                });
        }
    }
}
