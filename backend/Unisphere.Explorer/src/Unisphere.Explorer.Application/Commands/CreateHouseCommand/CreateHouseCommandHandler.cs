using ErrorOr;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Domain;

namespace Unisphere.Explorer.Application.Commands;

internal sealed class CreateHouseCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateHouseCommand, Guid>
{
    public async Task<ErrorOr<Guid>> Handle(CreateHouseCommand command, CancellationToken cancellationToken)
    {
        var house = new House
        {
            Description = command.Description,
        };

        context.Houses.Add(house);

        await context.SaveChangesAsync(cancellationToken);

        return house.Id;
    }
}
