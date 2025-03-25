using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Unisphere.Core.Application.Abstractions;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Domain.Exceptions;

namespace Unisphere.Explorer.Application.Commands;

internal sealed class DeleteHouseCommandHandler(IExplorerDbContext dbContext) : ICommandHandler<DeleteHouseCommand, bool>
{
    public async Task<ErrorOr<bool>> Handle(DeleteHouseCommand command, CancellationToken cancellationToken)
    {
        var house = await dbContext.Houses
            .Where(house => house.Id == command.HouseId)
            .SingleOrDefaultAsync(cancellationToken);

        if (house is null)
        {
            return HouseErrors.NotFound(command.HouseId.GetValueOrDefault());
        }

        dbContext.Houses.Remove(house);

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
