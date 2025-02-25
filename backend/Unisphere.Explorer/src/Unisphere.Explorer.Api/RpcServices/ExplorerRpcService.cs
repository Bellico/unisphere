using Grpc.Core;
using Mapster;
using MediatR;
using Unisphere.Core.Common.Extensions;
using Unisphere.Core.Presentation;
using Unisphere.Explorer.Application.Queries;

namespace Unisphere.Explorer.Api.RpcServices;

internal sealed class ExplorerRpcService(ISender sender) : ExplorerService.ExplorerServiceBase
{
    public override Task<Empty> GetError(Empty request, ServerCallContext context)
    {
        throw new InvalidOperationException("Error business data");
    }

    public async override Task<GetHouseRpcResponse> GetHouseDetail(GetHouseRpcRequest request, ServerCallContext context)
    {
        var command = new GetHouseDetailQuery(request.Id.AsGuid());

        var result = await sender.Send(command, context.CancellationToken);

        return result.Match(
            success => success.Adapt<GetHouseRpcResponse>(),
            errors => throw ProblemDetailHelper.RpcProblem(errors));
    }
}
