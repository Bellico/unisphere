using Application.Abstractions.Authentication;
using MediatR.Pipeline;
using Unisphere.Core.Application.Exceptions;
using Unisphere.Explorer.Application.Abstractions;

namespace Unisphere.Explorer.Application.Behaviors;

public class AuthorizationPreProcessor<TRequest>(IUserDataScopeService userDataScopeService, IUserContextService userContextService)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var userId = userContextService.GetUserId();

        if (!userId.HasValue)
        {
            throw new ForbiddenAccessException();
        }

        if (request is IOwnerHouseRequirement command)
        {
            var isOwner = await userDataScopeService
                    .IsUserOwnerHouseAsync(userId.Value, command.HouseId.Value, cancellationToken)
                    .ConfigureAwait(false);

            if (!isOwner)
            {
                throw new ForbiddenAccessException();
            }
        }
    }
}
