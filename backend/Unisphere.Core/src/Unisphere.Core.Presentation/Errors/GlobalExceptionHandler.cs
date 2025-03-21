using FluentValidation;
using Grpc.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unisphere.Core.Application.Exceptions;

namespace Unisphere.Core.Presentation.Errors;

public partial class GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        LogExceptionError(logger, exception);

        var problemDetails = exception switch
        {
            ValidationException validationException => ProblemDetailHelper.CreateProblemDetails(validationException),
            ForbiddenAccessException forbiddenAccessException => ProblemDetailHelper.CreateProblemDetails(forbiddenAccessException),
            RpcException rpcException => ProblemDetailHelper.CreateProblemDetails(httpContext, rpcException, env),
            _ => ProblemDetailHelper.CreateProblemDetails(httpContext, exception, env),
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    [LoggerMessage(1, LogLevel.Error, "An unhandled exception has occurred")]
    static partial void LogExceptionError(ILogger logger, Exception exception);
}
