using Microsoft.Extensions.DependencyInjection;
using Unisphere.Core.Application;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Application.Services;

namespace Unisphere.Explorer.Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.RegisterApplicationCore();

        services.AddScoped<IUserDataScopeService, UserDataScopeService>();

        return services;
    }
}
