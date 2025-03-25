using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Unisphere.Identity.Application.Abstractions;
using Unisphere.Identity.Domain;

namespace Unisphere.Identity.Infrastructure;

public sealed class UnisphereIdentityDbContext(DbContextOptions<UnisphereIdentityDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options), IUnisphereIdentityDbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(UnisphereIdentityDbContext).Assembly);

        builder.HasDefaultSchema("identity");
    }
}
