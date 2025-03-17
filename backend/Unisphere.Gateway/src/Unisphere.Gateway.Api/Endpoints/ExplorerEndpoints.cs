using Unisphere.Explorer.Api.RpcServices;

namespace Unisphere.Gateway.Api;

internal static class ExplorerEndpoints
{
    public static IEndpointRouteBuilder MapExplorerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("explorer").RequireAuthorization();

        group.MapGet("houses", async (ExplorerService.ExplorerServiceClient explorerApiService, CancellationToken cancellationToken) =>
        {
            return (await explorerApiService.SearchHousesAsync(new SearchHousesRequest(), cancellationToken: cancellationToken)).Houses;
        });

        group.MapGet("houses/{houseId:guid}", async (Guid houseId, ExplorerService.ExplorerServiceClient explorerApiService, CancellationToken cancellationToken) =>
        {
            return await explorerApiService.GetHouseAsync(new GetHouseRequest { Id = houseId.ToString() }, cancellationToken: cancellationToken);
        });

        group.MapPost("houses", async (ExplorerService.ExplorerServiceClient explorerApiService, CreateHouseRequest request, CancellationToken cancellationToken) =>
        {
            var result = await explorerApiService.CreateHouseAsync(request, cancellationToken: cancellationToken);

            return Results.Created($"explorer/houses/{result.HouseId}", result.HouseId);
        })
        .Produces<string>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("houses/{houseId:guid}", async (ExplorerService.ExplorerServiceClient explorerApiService, Guid houseId, UpdateHouseRequest request, CancellationToken cancellationToken) =>
        {
            request.HouseId = houseId.ToString();
            await explorerApiService.UpdateHouseAsync(request, cancellationToken: cancellationToken);
        });

        group.MapDelete("houses/{houseId:guid}", async (ExplorerService.ExplorerServiceClient explorerApiService, Guid houseId, CancellationToken cancellationToken) =>
        {
            await explorerApiService.DeleteHouseAsync(new DeleteHouseRequest { HouseId = houseId.ToString() }, cancellationToken: cancellationToken);
        });

        return app;
    }
}
