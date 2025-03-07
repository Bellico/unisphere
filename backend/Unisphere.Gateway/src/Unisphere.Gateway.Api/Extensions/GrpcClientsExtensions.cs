using Unisphere.Explorer.Api.RpcServices;

namespace Unisphere.Gateway.Api.Extensions;

internal static class GrpcClientsExtensions
{
    public static void AddGrpcClients(this IServiceCollection services)
    {
        services.AddGrpcClient<ExplorerService.ExplorerServiceClient>(o =>
        {
            o.Address = new Uri("https://explorer");
        });
    }
}
