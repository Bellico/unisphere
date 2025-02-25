using System.Diagnostics;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Unisphere.Core.Presentation;

public static partial class ProblemDetailHelper
{
    private const string InternalServerErrorTitle = "An unhandled exception has occurred while executing the request";
    private const string BadRequestErrorTitle = "Bad Request";

    private static readonly Dictionary<int, string> ProblemTypes = new()
    {
        { 400, "https://tools.ietf.org/html/rfc7231#section-6.5.1" },
        { 401, "https://tools.ietf.org/html/rfc7235#section-3.1" },
        { 403, "https://tools.ietf.org/html/rfc7231#section-6.5.3" },
        { 404, "https://tools.ietf.org/html/rfc7231#section-6.5.4" },
        { 409, "https://tools.ietf.org/html/rfc7231#section-6.5.8" },
        { 500, "https://tools.ietf.org/html/rfc7231#section-6.6.1" },
    };

    public static ProblemDetails CreateProblemDetails(HttpContext context, Exception exception, IHostEnvironment env)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = ProblemTypes[StatusCodes.Status500InternalServerError],
            Title = InternalServerErrorTitle,
            Extensions = { },
        };

        if (!env.IsDevelopment())
        {
            return problemDetails;
        }

        problemDetails.Detail = exception.ToString();
        problemDetails.Extensions["traceId"] = Activity.Current?.Id;
        problemDetails.Extensions["requestId"] = context.TraceIdentifier;
        problemDetails.Extensions["data"] = exception.Data;

        return problemDetails;
    }

    public static ProblemDetails CreateProblemDetails(ValidationException validationException)
    {
        var validationErrors = validationException.Errors
                 .GroupBy(e => e.PropertyName)
                 .ToDictionary(f => f.Key, f => f.Select(u => u.ErrorMessage).ToList());

        var problemDetails = new ProblemDetails
        { 
            Status = StatusCodes.Status400BadRequest,
            Type = ProblemTypes[StatusCodes.Status400BadRequest],
            Title = BadRequestErrorTitle,
            Extensions =
            {
                { "errors", validationErrors },
            },
        };

        return problemDetails;
    }

    public static IResult Problem(IList<Error> errors)
    {
        if (errors.Count > 1)
        {
            return Results.ValidationProblem(
                errors.ToDictionary(k => k.Code, v => new[] { v.Description }),
                statusCode: GetStatusCode(errors[0].Type));
        }
        else
        {
            return Results.Problem(
                 title: errors[0].Code,
                 detail: errors[0].Description,
                 statusCode: GetStatusCode(errors[0].Type));
        }
    }

    private static int GetStatusCode(ErrorType errorType) =>
          errorType switch
          {
              ErrorType.Validation => StatusCodes.Status400BadRequest,
              ErrorType.NotFound => StatusCodes.Status404NotFound,
              ErrorType.Conflict => StatusCodes.Status409Conflict,
              ErrorType.Forbidden => StatusCodes.Status403Forbidden,
              ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
              _ => StatusCodes.Status500InternalServerError,
          };
}
