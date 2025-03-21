using AFactoring.Core.Middle.Definitions.Interfaces;
using Application.Abstractions.Authentication;
using MediatR.Pipeline;
using Unisphere.Explorer.Application.Abstractions;

namespace Unisphere.Explorer.Application.Behaviors;

public class AuthorizationPreProcessor<TRequest>(IUserDataScopeService userDataScopeService, IUserContextService userContextService)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (request is IOwnerHouseRequirement command)
        {
            var isOwner = await userDataScopeService
                    .IsUserOwnerHouseAsync(userContextService.GetUserId().Value, command.HouseId.Value, cancellationToken)
                    .ConfigureAwait(false);

            if (!isOwner)
            {
                // UnauthorizedAccessException ?
                throw new InvalidOperationException("User has not access");
            }
        }
    }
}
