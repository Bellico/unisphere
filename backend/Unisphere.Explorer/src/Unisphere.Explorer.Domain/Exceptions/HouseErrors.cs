using ErrorOr;

namespace Unisphere.Explorer.Domain.Exceptions;

public static class HouseErrors
{
    public static Error NotFound(Guid houseId) => Error.NotFound(
        "Houses.NotFound",
        $"The house with the Id = '{houseId}' was not found");
}
