using Grpc.Core;
using Mapster;
using MediatR;
using Unisphere.Explorer.Application.Queries;

namespace Unisphere.Explorer.Api.RpcServices;

internal class ExplorerRpcService(ISender sender) : ExplorerService.ExplorerServiceBase
{
    public override Task<Empty> GetError(Empty request, ServerCallContext context)
    {
        throw new InvalidOperationException("Error business data");
    }

    public async override Task<GetHouseRpcResponse> GetHouseDetail(GetHouseRpcRequest request, ServerCallContext context)
    {
        if (Guid.TryParse(request.Id, out Guid guiid))
        {
            var command = new GetHouseDetailQuery(guiid);

            var result = await sender.Send(command, context.CancellationToken);

            return result.Match(
                      success => success.Adapt<GetHouseRpcResponse>(),
                      errors => throw GrpcCustomResults.Problem(errors)
                  );
        }
        else
        {
            return new GetHouseRpcResponse
            {
                Id = "0",
                Description = "Invalid Id",
            };
        }
    }
}
