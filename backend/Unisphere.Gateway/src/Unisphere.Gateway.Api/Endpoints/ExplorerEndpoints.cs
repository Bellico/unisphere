using Unisphere.Explorer.Api.RpcServices;

namespace Unisphere.Gateway.Api;

internal static class ExplorerEndpoints
{
    public static IEndpointRouteBuilder MapExplorerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("explorer");

        group.MapGet("houses", async (ExplorerService.ExplorerServiceClient explorerApiService) =>
        {
            return (await explorerApiService.SearchHousesAsync(new SearchHousesRequest())).Houses;
        });

        group.MapGet("houses/{houseId:guid}", async (Guid houseId, ExplorerService.ExplorerServiceClient explorerApiService) =>
        {
            return await explorerApiService.GetHouseAsync(new GetHouseRequest { Id = houseId.ToString() });
        })
        .WithTags("tags toto")
        .WithName("name toto")
        .WithSummary("WithSummary toto")
        .WithDescription("WithDescription toto")
        .Produces<string>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        group.MapPost("houses", async (ExplorerService.ExplorerServiceClient explorerApiService, CreateHouseRequest request) =>
        {
            var result = await explorerApiService.CreateHouseAsync(request);

            return Results.Created($"explorer/houses/{result.HouseId}", result.HouseId);
        });

        group.MapPost("houses/{houseId:guid}", async (ExplorerService.ExplorerServiceClient explorerApiService, Guid houseId, UpdateHouseRequest request) =>
        {
            request.HouseId = houseId.ToString();
            await explorerApiService.UpdateHouseAsync(request);
        });

        group.MapDelete("houses/{houseId:guid}", async (ExplorerService.ExplorerServiceClient explorerApiService, Guid houseId) =>
        {
            await explorerApiService.DeleteHouseAsync(new DeleteHouseRequest { HouseId = houseId.ToString() });
        });

        return app;
    }
}
