using Microsoft.EntityFrameworkCore;

namespace Unisphere.Gateway.Api.Database;

internal sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("gateway");
    }
}
