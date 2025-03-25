using Microsoft.Extensions.DependencyInjection;
using Unisphere.Core.Application;

namespace Unisphere.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.RegisterApplicationCore();

        return services;
    }
}
