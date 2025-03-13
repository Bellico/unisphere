using FluentValidation;

namespace Unisphere.Explorer.Application.Commands;

public class DeleteHouseCommandValidator : AbstractValidator<DeleteHouseCommand>
{
    public DeleteHouseCommandValidator()
    {
        RuleFor(c => c.HouseId).NotNull();
    }
}
