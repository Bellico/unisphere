using FluentValidation;

namespace Unisphere.Explorer.Application.Queries;

public class GetHouseDetailQueryValidator : AbstractValidator<GetHouseDetailQuery>
{
    public GetHouseDetailQueryValidator()
    {
        RuleFor(c => c.HouseId)
            .Must(x => x == new Guid("981F20ED-C969-4B8C-B2DA-ACC1C38BB5A7") || x == new Guid("981F20ED-C969-4B8C-B2DA-ACC1C38BB5A8"))
            .NotNull();
    }
}
