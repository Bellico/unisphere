using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Unisphere.Core.Presentation.Interfaces;

namespace Unisphere.Core.Presentation.Extensions;

public static class EndpointExtensions
{
    public static RouteHandlerBuilder RequireOwnerPolicy(this RouteHandlerBuilder app)
    {
        return app.RequireAuthorization("owner");
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointGroupTypes = Assembly.GetCallingAssembly()
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IEndpointGroupBase)));

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is IEndpointGroupBase instance)
            {
                instance.Map(app);
            }
        }

        return app;
    }
}
