using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Unisphere.Core.Application.Abstractions;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Domain.Exceptions;

namespace Unisphere.Explorer.Application.Commands;

internal sealed class UpdateHouseCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateHouseCommand, bool>
{
    public async Task<ErrorOr<bool>> Handle(UpdateHouseCommand command, CancellationToken cancellationToken)
    {
        var house = await dbContext.Houses
            .Where(house => house.Id == command.HouseId)
            .SingleOrDefaultAsync(cancellationToken);

        if (house is null)
        {
            return HouseErrors.NotFound(command.HouseId.GetValueOrDefault());
        }

        house.Name = command.Name;
        house.Description = command.Description;

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
