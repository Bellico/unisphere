using Unisphere.Core.Presentation.Errors;
using Unisphere.Core.Presentation.Interceptors;

namespace Unisphere.Explorer.Api;

internal static class DependencyInjection
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddGrpc(c => c.Interceptors.Add<GrpcGlobalExceptionHandlerInterceptor>());
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
