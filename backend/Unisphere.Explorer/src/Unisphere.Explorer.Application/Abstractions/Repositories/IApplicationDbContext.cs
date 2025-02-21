using Microsoft.EntityFrameworkCore;
using Unisphere.Explorer.Domain;

namespace Unisphere.Explorer.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Booking> Bookings { get; }

    DbSet<House> Houses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
