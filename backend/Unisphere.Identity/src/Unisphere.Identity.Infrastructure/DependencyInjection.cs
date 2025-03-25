using CleanArchitecture.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Unisphere.Core.Infrastructure;
using Unisphere.Identity.Application.Abstractions;
using Unisphere.Identity.Domain;

namespace Unisphere.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterInfrastructureCore();

        var connectionString = configuration.GetConnectionString("db-identity");

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<UnisphereIdentityDbContext>();

        services.AddDbContext<IUnisphereIdentityDbContext, UnisphereIdentityDbContext>(
            options => options
                .UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "identity"))
                .UseSnakeCaseNamingConvention());

        services
            .AddHealthChecks()
            .AddNpgSql(connectionString);

        services.AddTransient<IIdentityService, IdentityService>();

        return services;
    }
}
