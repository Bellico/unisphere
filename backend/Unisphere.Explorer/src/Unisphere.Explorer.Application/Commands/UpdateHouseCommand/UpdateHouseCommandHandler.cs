using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Domain;

namespace Unisphere.Explorer.Application.Commands;

internal sealed class UpdateHouseCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateHouseCommand>
{
    public async Task Handle(UpdateHouseCommand command, CancellationToken cancellationToken)
    {
        var house = new House
        {
            Description = command.Description,
        };

        context.Houses.Add(house);

        await context.SaveChangesAsync(cancellationToken);
    }
}
