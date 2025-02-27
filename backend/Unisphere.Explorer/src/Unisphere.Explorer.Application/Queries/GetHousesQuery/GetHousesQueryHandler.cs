using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Application.Models;
using Unisphere.Explorer.Domain.Exceptions;

namespace Unisphere.Explorer.Application.Queries;

internal sealed partial class GetHousesQueryHandler(IApplicationDbContext context) : IQueryErrorOrHandler<GetHousesQuery, List<HouseModel>>
{
    public async Task<ErrorOr<List<HouseModel>>> Handle(GetHousesQuery query, CancellationToken cancellationToken)
    {
        if (query.HouseId is null)
        {
            return HouseErrors.NotFound(query.HouseId.GetValueOrDefault());
        }

        return await context.Houses
            .Where(u => u.Id == query.HouseId.Value)
            .Select(u => new HouseModel
            {
                Id = u.Id,
            })
            .ToListAsync(cancellationToken);
    }
}
