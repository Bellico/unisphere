using FluentValidation;

namespace Unisphere.Explorer.Application.Queries;

public class GetHouseDetailQueryValidator : AbstractValidator<GetHouseDetailQuery>
{
    public GetHouseDetailQueryValidator()
    {
        RuleFor(c => c.HouseId)
            .NotNull();
    }
}
