using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Unisphere.Core.Application.Behaviors;

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
        });

        return services;
    }
}
