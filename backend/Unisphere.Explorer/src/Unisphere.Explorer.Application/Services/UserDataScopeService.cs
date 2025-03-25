using Microsoft.EntityFrameworkCore;
using Unisphere.Explorer.Application.Abstractions;

namespace Unisphere.Explorer.Application.Services;

public class UserDataScopeService(IExplorerDbContext dbContext) : IUserDataScopeService
{
    public Task<bool> IsUserOwnerHouseAsync(Guid userId, Guid houseId, CancellationToken cancellationToken)
    {
        return dbContext.Houses
            .Where(h => h.Id == houseId)
            .Where(h => h.AuthorId == userId)
            .AnyAsync(cancellationToken);
    }
}
