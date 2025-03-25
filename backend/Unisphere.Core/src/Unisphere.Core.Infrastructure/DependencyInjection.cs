using Application.Abstractions.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Unisphere.Core.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureCore(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IUserContextService, UserContextService>();

        return services;
    }
}
