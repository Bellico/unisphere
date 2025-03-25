using Microsoft.EntityFrameworkCore;
using Unisphere.Core.Application.Abstractions;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Application.Models;

namespace Unisphere.Explorer.Application.Queries;

internal sealed class SearchHousesQueryHandler(IExplorerDbContext context) : IQueryHandler<SearchHousesQuery, IList<HouseModel>>
{
    public async Task<IList<HouseModel>> Handle(SearchHousesQuery command, CancellationToken cancellationToken)
    {
        return await context.Houses.Select(s => new HouseModel
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
        }).ToListAsync(cancellationToken);
    }
}
