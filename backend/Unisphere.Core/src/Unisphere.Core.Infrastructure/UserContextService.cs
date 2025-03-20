using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Unisphere.Core.Infrastructure;

public sealed class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid? GetUserId()
    {
        var principal = _httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("User id is unavailable");

        var userId = principal.FindFirst("sub")?.Value;

        return Guid.TryParse(userId, out Guid parsedUserId) ?
            parsedUserId :
            throw new InvalidOperationException("User id is unavailable");
    }
}
