using FluentValidation;

namespace Unisphere.Explorer.Application.Commands;

public class UpdateHouseCommandValidator : AbstractValidator<UpdateHouseCommand>
{
    public UpdateHouseCommandValidator()
    {
        RuleFor(c => c.Description).NotEmpty().MaximumLength(255);
    }
}
