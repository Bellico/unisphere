using Microsoft.EntityFrameworkCore;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Domain;

namespace Unisphere.Explorer.Infrastructure;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Booking> Bookings { get; set; }

    public DbSet<House> Houses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema("explorer");
    }
}
