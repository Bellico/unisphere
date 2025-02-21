using Unisphere.Explorer.Api.RpcServices;

namespace Unisphere.Gateway.Api;

internal static class GrpcClients
{
    public static void AddGrpcClients(this IServiceCollection services)
    {
        services.AddGrpcClient<ExplorerService.ExplorerServiceClient>(o =>
        {
            o.Address = new Uri("https://explorer");
        });
    }
}
