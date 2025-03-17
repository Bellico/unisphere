using ErrorOr;
using Unisphere.Core.Application.Abstractions;
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
            Name = command.Description,
            Description = command.Description,
            PhysicalAddress = PhysicalAddress.Empty,
            AuthorId = Guid.NewGuid(),
            ImageUrl = new Uri("http://image.com"),
            Notation = Notation.Zero,
        };

        context.Houses.Add(house);

        await context.SaveChangesAsync(cancellationToken);

        return house.Id;
    }
}
