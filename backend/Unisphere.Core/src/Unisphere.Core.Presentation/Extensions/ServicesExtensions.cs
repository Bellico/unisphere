using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Unisphere.Core.Presentation.Errors;
using Unisphere.Core.Presentation.Interceptors;
using Unisphere.Core.Presentation.Middlewares;
using Unisphere.ServiceDefaults.Extensions;

namespace Unisphere.Core.Presentation.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddUnisphereCore(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddGrpc(c => c.Interceptors.Add<GrpcGlobalExceptionHandlerInterceptor>());

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>()
                .AddProblemDetails();

        return builder.Services;
    }

    public static WebApplication UseUnisphereCore(this WebApplication app)
    {
        app.UseExceptionHandler();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseMiddleware<RequestContextLoggingMiddleware>();

        app.MapDefaultEndpoints();

        return app;
    }
}
