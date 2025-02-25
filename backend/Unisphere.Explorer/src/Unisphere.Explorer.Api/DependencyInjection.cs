using Shared.Presentation;

namespace Unisphere.Explorer.Api;

internal static class DependencyInjection
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection services)
    {
        services.AddGrpc(c => c.Interceptors.Add<GrpcGlobalExceptionHandlerInterceptor>());

        services.AddEndpointsApiExplorer();

        //services.AddSwaggerGen();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
