using Microsoft.AspNetCore.Routing;

namespace Unisphere.Core.Presentation.Interfaces;

public interface IEndpointGroupBase
{
    public IEndpointRouteBuilder Map(IEndpointRouteBuilder app);
}
