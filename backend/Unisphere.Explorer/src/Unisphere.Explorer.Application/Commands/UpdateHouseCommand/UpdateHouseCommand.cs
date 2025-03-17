using Unisphere.Core.Application.Abstractions;

namespace Unisphere.Explorer.Application.Commands;

public sealed class UpdateHouseCommand : ICommand<bool>
{
    public Guid? HouseId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
