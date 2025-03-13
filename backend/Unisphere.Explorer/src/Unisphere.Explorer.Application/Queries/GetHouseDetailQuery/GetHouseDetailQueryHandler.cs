using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Application.Models;
using Unisphere.Explorer.Domain.Exceptions;

namespace Unisphere.Explorer.Application.Queries;

internal sealed partial class GetHouseDetailQueryHandler(IApplicationDbContext dbContext) : IQueryErrorOrHandler<GetHouseDetailQuery, HouseModel>
{
    public async Task<ErrorOr<HouseModel>> Handle(GetHouseDetailQuery query, CancellationToken cancellationToken)
    {
        var house = await dbContext.Houses
            .Where(house => house.Id == query.HouseId)
            .Select(house => new HouseModel
            {
                Id = house.Id,
                Name = house.Name,
                Description = house.Description,
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (house is null)
        {
            return HouseErrors.NotFound(query.HouseId.GetValueOrDefault());
        }

        return house;
    }
}
