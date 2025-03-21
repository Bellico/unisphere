namespace Unisphere.Explorer.Application.Abstractions;

public interface IUserDataScopeService
{
    Task<bool> IsUserOwnerHouseAsync(Guid userId, Guid houseId, CancellationToken cancellationToken);
}
