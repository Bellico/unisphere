using FluentValidation;

namespace Unisphere.Explorer.Application.Commands;

public class CreateHouseCommandValidator : AbstractValidator<CreateHouseCommand>
{
    public CreateHouseCommandValidator()
    {
        RuleFor(c => c.Description).NotEmpty().MaximumLength(255);
    }
}
