using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Client.WebIntegration;
using OpenIddict.Server.AspNetCore;
using Unisphere.Core.Common.Constants;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Unisphere.Gateway.Api;

internal static class AuthorizationEndpoints
{
    public static IEndpointRouteBuilder MapAuthorizationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("login", (string? returnUrl) =>
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = returnUrl,
            };

            return Results.Challenge(properties, [OpenIddictClientWebIntegrationConstants.Providers.Spotify]);
        });

        app.MapGet("connect/authorization", async (HttpContext context, IOpenIddictApplicationManager manager, CancellationToken cancellationToken) =>
        {
            var principal = (await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme))?.Principal;

            if (principal is null)
            {
                return Results.Forbid();
            }

            var request = context.GetOpenIddictServerRequest();

            if (request.IsAuthorizationCodeFlow())
            {
                var identifier = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                return await ProcessAuthorizationCodeFlowAsync(request, manager, identifier, cancellationToken);
            }

            return Forbid(Errors.UnsupportedGrantType, $"The specified grant type {request.GrantType} is not supported");
        });

        app.MapPost("connect/token", async (HttpContext context, IOpenIddictApplicationManager manager) =>
         {
             var request = context.GetOpenIddictServerRequest()!;

             if (request.IsClientCredentialsGrantType())
             {
                 return ProcessClientCredentialsGrantTypeAsync(request);
             }

             if (request.IsAuthorizationCodeGrantType())
             {
                 return await ProcessRefreshTokenOrGrantTypeAsync(context);
             }

             if (request.IsRefreshTokenGrantType())
             {
                 return await ProcessRefreshTokenOrGrantTypeAsync(context);
             }

             return Forbid(Errors.UnsupportedGrantType, $"The specified grant type {request.GrantType} is not supported");
         });

        app.MapGet("callback", (HttpContext context) =>
        {
            var query = QueryHelpers.ParseQuery(context.Request.QueryString.Value);

            return Results.Ok(query.GetValueOrDefault("code"));
        });

        app.MapGet("/callback-spotify", async (HttpContext context) =>
        {
            var result = await context.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);

            if (result is null)
            {
                return Results.Forbid();
            }

            var identity = new ClaimsIdentity(
                authenticationType: "ExternalLogin",
                nameType: ClaimTypes.Name,
                roleType: ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.Principal!.FindFirst("id")!.Value));

            var properties = new AuthenticationProperties
            {
                RedirectUri = result.Properties?.RedirectUri ?? "/",
            };

            return Results.SignIn(new ClaimsPrincipal(identity), properties, authenticationScheme: CookieAuthenticationDefaults.AuthenticationScheme);
        });

        return app;
    }

    private static async Task<IResult> ProcessAuthorizationCodeFlowAsync(
        OpenIddictRequest request,
        IOpenIddictApplicationManager manager,
        string identifier,
        CancellationToken cancellationToken)
    {
        var application = await manager.FindByClientIdAsync(request.ClientId, cancellationToken);

        if (application == null)
        {
            return Forbid(Errors.InvalidClient, "The provided application does not exists.");
        }

        var userId = "01958078-19db-7eed-adf7-94b77900c8ca"; // Use identifier

        var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType);
        identity.AddClaim(Claims.Subject, userId);
        identity.AddClaim(Claims.Name, "Franck");
        identity.AddClaim(Claims.Email, "franck@hotmail.fr");
        identity.AddClaim(Claims.PreferredUsername, identifier);

        var claimsPrincipal = new ClaimsPrincipal(identity);

        claimsPrincipal.SetScopes(request.GetScopes());

        claimsPrincipal.SetDestinations(static claim => claim.Type switch
        {
            Claims.Name => [Destinations.AccessToken, Destinations.IdentityToken],
            _ => [Destinations.AccessToken],
        });

        return Results.SignIn(claimsPrincipal, authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static IResult ProcessClientCredentialsGrantTypeAsync(OpenIddictRequest request)
    {
        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        identity.AddClaim(new Claim(Claims.Subject, request.ClientId));

        identity.SetDestinations(static claim => claim.Type switch
        {
            // Allow the "name" claim to be stored in both the access and identity tokens
            // when the "profile" scope was granted (by calling principal.SetScopes(...)).
            Claims.Name when claim.Subject.HasScope(Scopes.Profile) => [Destinations.AccessToken, Destinations.IdentityToken],
            // Otherwise, only store the claim in the access tokens.
            _ => [Destinations.AccessToken],
        });

        var claimsPrincipal = new ClaimsPrincipal(identity);

        claimsPrincipal.SetScopes(UnisphereConstants.Scopes.AuthorizedScopes.Intersect(request.GetScopes()));

        // claimsPrincipal.SetResources(Audience);
        // Set the list of scopes granted to the client application.
        // claimsPrincipal.SetScopes(AuthorizedScopesForServerApplications.Intersect(request.GetScopes()));
        return Results.SignIn(claimsPrincipal, authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static async Task<IResult> ProcessRefreshTokenOrGrantTypeAsync(HttpContext context)
    {
        // Retrieve the claims principal stored in the refresh token.
        var result = await context.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        if (result is null)
        {
            return Results.Forbid();
        }

        return Results.SignIn(result.Principal, new AuthenticationProperties(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static IResult Forbid(string error, string errorDescription)
    {
        return Results.Forbid(
            authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme],
            properties: new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription,
            }));
    }
}
