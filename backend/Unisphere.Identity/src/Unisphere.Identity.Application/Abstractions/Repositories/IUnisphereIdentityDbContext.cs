namespace Unisphere.Identity.Application.Abstractions;

public interface IUnisphereIdentityDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
