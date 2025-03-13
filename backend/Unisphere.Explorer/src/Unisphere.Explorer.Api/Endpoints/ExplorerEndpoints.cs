using MediatR;
using Unisphere.Core.Presentation;
using Unisphere.Explorer.Application.Queries;

namespace Unisphere.Explorer.Api.Endpoints;

internal static class ExplorerEndpoints
{
    /// <summary>
    /// Exemple endpoint instead of gRPC.
    /// </summary>
    /// <param name="app">A IEndpointRouteBuilder.</param>
    /// <returns>IEndpointRouteBuilder.</returns>
    public static IEndpointRouteBuilder MapExplorerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/houses/{houseId:guid}", async (Guid houseId, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetHouseDetailQuery(houseId);

            var result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, ProblemDetailHelper.Problem);
        });

        return app;
    }
}
