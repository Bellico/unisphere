using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Unisphere.Explorer.Api.Middlewares;

internal class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            var problemDetails = CreateProblemDetails(validationException);

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        return false;
    }

    private static ProblemDetails CreateProblemDetails(ValidationException validationException)
    {
        var validationErrors = validationException.Errors
                 .GroupBy(e => e.PropertyName)
                 .ToDictionary(f => f.Key, f => f.Select(u => new
                 {
                     u.ErrorCode,
                     u.PropertyName,
                     u.ErrorMessage,
                 }).ToList());

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Title = "BadRequest",
            Extensions =
            {
                { "errors", validationErrors },
            },
        };

        return problemDetails;
    }
}
