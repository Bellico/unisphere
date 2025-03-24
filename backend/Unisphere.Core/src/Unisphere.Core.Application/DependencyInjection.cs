using System.Reflection;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Unisphere.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationCore(this IServiceCollection services)
    {
        var assemblies = new Assembly[] { Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly() };

        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);

            foreach (var type in assemblies.GetPipelineTypes(typeof(IPipelineBehavior<,>)))
            {
                config.AddOpenBehavior(type);
            }

            foreach (var type in assemblies.GetPipelineTypes(typeof(IRequestPreProcessor<>)))
            {
                config.AddOpenRequestPreProcessor(type);
            }

            foreach (var type in assemblies.GetPipelineTypes(typeof(IRequestPostProcessor<,>)))
            {
                config.AddOpenRequestPostProcessor(type);
            }
        });

        return services;
    }

    private static IEnumerable<Type> GetPipelineTypes(this Assembly[] assemblies, Type type) => assemblies
            .SelectMany(asm => asm.GetTypes())
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type));
}
