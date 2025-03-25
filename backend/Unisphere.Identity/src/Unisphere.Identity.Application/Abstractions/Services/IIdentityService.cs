using ErrorOr;
using Unisphere.Identity.Domain;

namespace Unisphere.Identity.Application.Abstractions;

public interface IIdentityService
{
    Task<User?> GetUserNameAsync(Guid userId);

    Task<ErrorOr<Guid>> CreateUserAsync(string userName, string password);

    Task<ErrorOr<bool>> DeleteUserAsync(Guid userId);
}
