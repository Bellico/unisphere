using Microsoft.Extensions.DependencyInjection;
using Unisphere.Core.Presentation.Interceptors;

namespace Unisphere.Gateway.Api.Extensions;

public static class GrpcClientsExtensions
{
    public static IServiceCollection AddGrpcServiceClient<TClient>(this IServiceCollection services, string serviceName) where TClient : class
    {
        services.EnsureInterceptorRegistration();

        services.AddGrpcClient<TClient>(o =>
        {
            o.Address = new Uri($"https://{serviceName}");
        })
        .AddInterceptor<GrpcAuthenticationInterceptor>()
        .AddStandardResilienceHandler();

        return services;
    }

    private static IServiceCollection EnsureInterceptorRegistration(this IServiceCollection services)
    {
        bool isRegistry = services.Any(s => s.ServiceType == typeof(GrpcAuthenticationInterceptor));

        if (!isRegistry)
        {
            services.AddScoped<GrpcAuthenticationInterceptor>();
        }

        return services;
    }
}
