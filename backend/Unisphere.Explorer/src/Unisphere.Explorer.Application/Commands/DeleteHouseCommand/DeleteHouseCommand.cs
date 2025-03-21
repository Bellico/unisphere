using AFactoring.Core.Middle.Definitions.Interfaces;
using Unisphere.Core.Application.Abstractions;

namespace Unisphere.Explorer.Application.Commands;

public sealed record DeleteHouseCommand : ICommand<bool>, IOwnerHouseRequirement
{
    public Guid? HouseId { get; set; }
}
