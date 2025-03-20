using Application.Abstractions.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Unisphere.Core.Infrastructure;
using Unisphere.Explorer.Application.Abstractions;

namespace Unisphere.Explorer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("db-explorer");

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(
            options => options
                .UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "explorer"))
                .UseSnakeCaseNamingConvention());

        services
            .AddHealthChecks()
            .AddNpgSql(connectionString);

        services.AddScoped<IUserContextService, UserContextService>();

        return services;
    }
}
