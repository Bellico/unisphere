using MediatR;
using Unisphere.Explorer.Application.Queries;

namespace Unisphere.Explorer.Api.Endpoints;

internal static class ExplorerEndpoints
{
    public static IEndpointRouteBuilder MapExplorerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/error", () =>
        {
            throw new InvalidOperationException("Error business data");
        });

        app.MapGet("/api/houses/{houseId:guid}", async (Guid houseId, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetHouseDetailQuery(houseId);

            var result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });

        return app;
    }
}
