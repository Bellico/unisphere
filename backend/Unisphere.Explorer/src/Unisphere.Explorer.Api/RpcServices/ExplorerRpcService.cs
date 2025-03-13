using Grpc.Core;
using Mapster;
using MediatR;
using Unisphere.Core.Common.Extensions;
using Unisphere.Core.Presentation;
using Unisphere.Explorer.Api.RpcServices;
using Unisphere.Explorer.Application.Commands;
using Unisphere.Explorer.Application.Queries;

namespace Unisphere.Explorer.Api.Services;

internal sealed class ExplorerRpcService(ISender sender) : ExplorerService.ExplorerServiceBase
{
    public override async Task<SearchHousesResponse> SearchHouses(SearchHousesRequest request, ServerCallContext context)
    {
        var command = new SearchHousesQuery();

        var result = await sender.Send(command, context.CancellationToken);

        return new SearchHousesResponse
        {
            Houses = { result.Adapt<IEnumerable<GetHouseResponse>>() },
        };
    }

    public override async Task<GetHouseResponse> GetHouse(GetHouseRequest request, ServerCallContext context)
    {
        var command = new GetHouseDetailQuery(request.Id.AsGuid());

        var result = await sender.Send(command, context.CancellationToken);

        return result.OnSuccess(s => s.Adapt<GetHouseResponse>());
    }

    public override async Task<CreateHouseResponse> CreateHouse(CreateHouseRequest request, ServerCallContext context)
    {
        var command = new CreateHouseCommand { Name = request.Name, Description = request.Description };

        var result = await sender.Send(command, context.CancellationToken);

        return result.OnSuccess(s => new CreateHouseResponse { HouseId = s.ToString() });
    }

    public override async Task<Empty> UpdateHouse(UpdateHouseRequest request, ServerCallContext context)
    {
        var command = new UpdateHouseCommand { HouseId = request.HouseId.AsGuid(), Name = request.Name, Description = request.Description };

        var result = await sender.Send(command, context.CancellationToken);

        return result.OnSuccess(_ => new Empty());
    }

    public override async Task<Empty> DeleteHouse(DeleteHouseRequest request, ServerCallContext context)
    {
        var command = new DeleteHouseCommand { HouseId = request.HouseId.AsGuid() };

        var result = await sender.Send(command, context.CancellationToken);

        return result.OnSuccess(_ => new Empty());
    }
}
