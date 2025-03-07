﻿using Unisphere.Explorer.Api.RpcServices;

namespace Unisphere.Gateway.Api;

internal static class ExplorerEndpoints
{
    public static IEndpointRouteBuilder MapExplorerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("explorer");

        group.MapGet("error", async (ExplorerService.ExplorerServiceClient explorerApiService) =>
        {
            await explorerApiService.GetErrorAsync(new Empty());
        });

        group.MapGet("houses/{houseId:guid}", async (Guid houseId, ExplorerService.ExplorerServiceClient explorerApiService) =>
        {
            var result = await explorerApiService.GetHouseDetailAsync(new GetHouseRpcRequest { Id = houseId.ToString() });
            return Results.Json(result);
        })
        .WithTags("tags toto")
        .WithName("name toto")
        .WithSummary("WithSummary toto")
        .WithDescription("WithDescription toto")
        .Produces<string>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        // .RequirePermission("Permissions.Basket.Checkout")
        .RequireAuthorization();

        return app;
    }
}
