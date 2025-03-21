using Unisphere.Core.Application.Abstractions;
using Unisphere.Explorer.Application.Abstractions;

namespace Unisphere.Explorer.Application.Commands;

public sealed record UpdateHouseCommand : ICommand<bool>, IOwnerHouseRequirement
{
    public Guid? HouseId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
