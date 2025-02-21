using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Unisphere.Explorer.Api.Middlewares;

internal sealed partial class GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
     HttpContext httpContext,
     Exception exception,
     CancellationToken cancellationToken)
    {
        LogExceptionError(logger, exception);

        var problemDetails = CreateProblemDetails(httpContext, exception);

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
    {
        var reasonPhrase = ReasonPhrases.GetReasonPhrase(context.Response.StatusCode);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = reasonPhrase ?? "An unhandled exception has occurred while executing the request.",
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
 
    [LoggerMessage(1, LogLevel.Error, "An unhandled exception has occurred")]
    static partial void LogExceptionError(ILogger logger, Exception exception);
}
