using Unisphere.Core.Application.Abstractions;

namespace Unisphere.Explorer.Application.Commands;

public sealed record CreateHouseCommand : ICommand<Guid>
{
    public string Name { get; set; }

    public string Description { get; set; }
}
