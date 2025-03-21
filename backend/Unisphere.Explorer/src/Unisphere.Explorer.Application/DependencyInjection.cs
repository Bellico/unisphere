using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Unisphere.Core.Application.Behaviors;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Application.Behaviors;
using Unisphere.Explorer.Application.Services;

namespace Unisphere.Explorer.Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            config.AddOpenRequestPreProcessor(typeof(RequestValidationPreProcessor<>));
            config.AddOpenRequestPreProcessor(typeof(AuthorizationPreProcessor<>));
        });

        services.AddScoped<IUserDataScopeService, UserDataScopeService>();

        return services;
    }
}
