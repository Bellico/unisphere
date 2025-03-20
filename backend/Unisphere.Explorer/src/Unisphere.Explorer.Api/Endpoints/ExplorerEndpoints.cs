using MediatR;
using Unisphere.Core.Presentation.Errors;
using Unisphere.Explorer.Application.Commands;
using Unisphere.Explorer.Application.Queries;

namespace Unisphere.Explorer.Api.Endpoints;

internal static class ExplorerEndpoints
{
    public static IEndpointRouteBuilder MapExplorerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/houses", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new SearchHousesQuery();

            return await sender.Send(command, cancellationToken);
        });

        app.MapGet("/api/houses/{houseId:guid}", async (Guid houseId, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GetHouseDetailQuery(houseId);

            var result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, ProblemDetailHelper.Problem);
        });

        app.MapPost("/api/houses", async (ISender sender, CreateHouseCommand request, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(request, cancellationToken);

            return result.Match(houseId => Results.Created($"/api/explorer/houses/{houseId}", houseId), ProblemDetailHelper.Problem);
        })
        .Produces<string>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapPut("/api/houses/{houseId:guid}", async (ISender sender, Guid houseId, UpdateHouseCommand request, CancellationToken cancellationToken) =>
        {
            var command = request with { HouseId = houseId };

            await sender.Send(command, cancellationToken);
        });

        app.MapDelete("/api/houses/{houseId:guid}", async (ISender sender, Guid houseId, CancellationToken cancellationToken) =>
        {
            var command = new DeleteHouseCommand { HouseId = houseId };

            await sender.Send(command, cancellationToken);
        });

        return app;
    }
}
