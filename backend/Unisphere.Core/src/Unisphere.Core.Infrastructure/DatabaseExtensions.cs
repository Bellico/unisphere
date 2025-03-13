using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Unisphere.Core.Infrastructure;

public static class DatabaseExtensions
{
    public static async Task ConfigureDatabaseAsync<TDbContext>(this IServiceProvider services) where TDbContext : DbContext
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        await dbContext.EnsureDatabaseAsync();
        await dbContext.ApplyMigrationsAsync();
    }

    private static async Task EnsureDatabaseAsync(this DbContext dbContext)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            if (!await dbCreator.ExistsAsync())
            {
                await dbCreator.CreateAsync();
            }
        });
    }

    private static async Task ApplyMigrationsAsync(this DbContext dbContext) => await dbContext.Database.MigrateAsync();
}
