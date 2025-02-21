using Unisphere.Explorer.Application.Abstractions;

namespace Unisphere.Explorer.Application.Commands;

public sealed class UpdateHouseCommand : ICommand
{
    public string Description { get; set; }
}
