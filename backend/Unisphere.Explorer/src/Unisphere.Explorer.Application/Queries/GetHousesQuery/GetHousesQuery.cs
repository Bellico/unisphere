using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Application.Models;

namespace Unisphere.Explorer.Application.Queries;

public sealed record GetHousesQuery(Guid? HouseId) : IQueryErrorOr<List<HouseModel>>;
