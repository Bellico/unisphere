using FluentValidation;

namespace Unisphere.Explorer.Application.Commands;

internal sealed class DeleteHouseCommandValidator : AbstractValidator<DeleteHouseCommand>
{
    public DeleteHouseCommandValidator()
    {
        RuleFor(c => c.HouseId).NotNull();
    }
}
