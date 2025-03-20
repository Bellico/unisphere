using FluentValidation;

namespace Unisphere.Explorer.Application.Commands;

internal sealed class UpdateHouseCommandValidator : AbstractValidator<UpdateHouseCommand>
{
    public UpdateHouseCommandValidator()
    {
        RuleFor(c => c.HouseId).NotNull();
        RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Description).NotEmpty().MaximumLength(255);
    }
}
