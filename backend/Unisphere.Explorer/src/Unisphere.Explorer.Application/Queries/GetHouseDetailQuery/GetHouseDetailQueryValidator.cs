using FluentValidation;

namespace Unisphere.Explorer.Application.Queries;

internal sealed class GetHouseDetailQueryValidator : AbstractValidator<GetHouseDetailQuery>
{
    public GetHouseDetailQueryValidator()
    {
        RuleFor(c => c.HouseId)
            .NotNull();
    }
}
