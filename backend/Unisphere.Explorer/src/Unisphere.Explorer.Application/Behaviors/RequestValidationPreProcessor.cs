using FluentValidation;
using MediatR.Pipeline;

namespace Unisphere.Explorer.Application.Behaviors;

public class RequestValidationPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationPreProcessor(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var validationResults = await Task.WhenAll(
             _validators.Select(validator => validator.ValidateAsync(request)));

        var validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        if (validationFailures.Length > 0)
        {
            throw new ValidationException(validationFailures);
        }
    }
}
