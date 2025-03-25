using Microsoft.EntityFrameworkCore;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Domain;

namespace Unisphere.Explorer.Infrastructure;

public sealed class ExplorerDbContext(DbContextOptions<ExplorerDbContext> options) : DbContext(options), IExplorerDbContext
{
    public DbSet<Booking> Bookings { get; set; }

    public DbSet<House> Houses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExplorerDbContext).Assembly);

        modelBuilder.HasDefaultSchema("explorer");
    }
}
