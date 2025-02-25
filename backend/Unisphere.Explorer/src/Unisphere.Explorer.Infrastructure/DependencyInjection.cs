using Microsoft.Extensions.DependencyInjection;
using Unisphere.Explorer.Application.Abstractions;

namespace Unisphere.Explorer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
    {
#pragma warning disable CS8603 // Possible null reference return.
        services.AddSingleton<IApplicationDbContext>((s) => null);
#pragma warning restore CS8603 // Possible null reference return.

        return services;
    }
}
