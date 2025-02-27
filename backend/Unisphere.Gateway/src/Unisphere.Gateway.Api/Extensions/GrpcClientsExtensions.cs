using Unisphere.Explorer.Api.RpcServices;

namespace Unisphere.Gateway.Api.Extensions;

internal static class GrpcClientsExtensions
{
    public static void AddGrpcClients(this IServiceCollection services)
    {
        services.AddGrpcClient<ExplorerService.ExplorerServiceClient>(o =>
        {
#pragma warning disable S1075 // URIs should not be hardcoded
            o.Address = new Uri("https://explorer");
#pragma warning restore S1075 // URIs should not be hardcoded
        });
    }
}
