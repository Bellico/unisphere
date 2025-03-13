using Unisphere.Explorer.Application.Abstractions;
using Unisphere.Explorer.Application.Models;

namespace Unisphere.Explorer.Application.Queries;

public sealed record SearchHousesQuery() : IQuery<IList<HouseModel>>;
