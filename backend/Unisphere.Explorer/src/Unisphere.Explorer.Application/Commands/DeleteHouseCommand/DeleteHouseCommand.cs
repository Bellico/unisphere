using Unisphere.Core.Application.Abstractions;
using Unisphere.Explorer.Application.Abstractions;

namespace Unisphere.Explorer.Application.Commands;

public sealed record DeleteHouseCommand : ICommand<bool>, IOwnerHouseRequirement
{
    public Guid? HouseId { get; set; }
}
