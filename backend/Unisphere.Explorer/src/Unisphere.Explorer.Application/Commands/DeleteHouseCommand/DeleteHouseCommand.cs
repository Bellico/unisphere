using Unisphere.Core.Application.Abstractions;

namespace Unisphere.Explorer.Application.Commands;

public sealed record DeleteHouseCommand : ICommand<bool>
{
    public Guid? HouseId { get; set; }
}
