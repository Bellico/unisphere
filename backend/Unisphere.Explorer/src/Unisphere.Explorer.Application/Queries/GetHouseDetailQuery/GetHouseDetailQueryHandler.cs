using ErrorOr;
using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Application.Models;
using Unisphere.Explorer.Domain;

namespace Unisphere.Explorer.Application.Queries;

internal sealed partial class GetHouseDetailQueryHandler() : IQueryErrorOrHandler<GetHouseDetailQuery, HouseModel>
{
    public async Task<ErrorOr<HouseModel>> Handle(GetHouseDetailQuery query, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);

        if (query.HouseId == new Guid("981F20ED-C969-4B8C-B2DA-ACC1C38BB5A8"))
        {
            return HouseErrors.NotFound(query.HouseId.GetValueOrDefault());
        }

        return new HouseModel
        {
            Id = query.HouseId.Value,
            Description = "A beautiful house",
        };
    }
}
