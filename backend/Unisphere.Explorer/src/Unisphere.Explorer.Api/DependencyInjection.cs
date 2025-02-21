using Unisphere.Explorer.Api.Middlewares;

namespace Unisphere.Explorer.Api;

public static class DependencyInjection
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection services)
    {
        services.AddGrpc(c => c.Interceptors.Add<GrpcGlobalExceptionHandlerInterceptor>());

        services.AddEndpointsApiExplorer();
        //services.AddSwaggerGen();

        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
