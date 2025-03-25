using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Unisphere.Identity.Application.Abstractions;
using Unisphere.Identity.Domain;

namespace CleanArchitecture.Infrastructure.Identity;

public class IdentityService(UserManager<User> userManager) : IIdentityService
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<User?> GetUserNameAsync(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<ErrorOr<Guid>> CreateUserAsync(string userName, string password)
    {
        var user = new User
        {
            UserName = userName,
            Email = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
        }

        return user.Id;
    }

    public async Task<ErrorOr<bool>> DeleteUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
        }

        return true;
    }
}
