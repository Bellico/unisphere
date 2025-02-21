using Microsoft.Extensions.DependencyInjection;
using Unisphere.Explorer.Application.Abstractions;

namespace Unisphere.Explorer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IApplicationDbContext>((s) => null);

        return services;
    }
}
