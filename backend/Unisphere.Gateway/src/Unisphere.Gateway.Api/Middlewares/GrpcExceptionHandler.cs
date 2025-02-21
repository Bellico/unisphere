using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Grpc.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Unisphere.Gateway.Api.Middlewares;

internal sealed partial class GrpcExceptionHandler(IHostEnvironment env, ILogger<GrpcExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
     HttpContext httpContext,
     Exception exception,
     CancellationToken cancellationToken)
    {
        if (exception is RpcException rpcException)
        {
            LogExceptionError(logger, rpcException);

            var problemDetails = CreateProblemDetails(httpContext, rpcException);

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        return false;
    }

    private ProblemDetails CreateProblemDetails(HttpContext context, RpcException exception)
    {
        var problemDetails = new ProblemDetails
        {
            Status = MapGrpcToHttpStatus(exception.StatusCode),
            Title = exception.Trailers.GetValue("error-code") ?? "An unhandled exception has occurred while executing the request.",
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

    private int MapGrpcToHttpStatus(StatusCode statusCode) => statusCode switch
    {
        StatusCode.NotFound => StatusCodes.Status404NotFound,
        StatusCode.InvalidArgument => StatusCodes.Status400BadRequest,
        StatusCode.PermissionDenied => StatusCodes.Status403Forbidden,
        StatusCode.Unauthenticated => StatusCodes.Status401Unauthorized,
        StatusCode.Internal => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError
    };

    [LoggerMessage(1, LogLevel.Error, "An gRPC unhandled exception has occurred")]
    static partial void LogExceptionError(ILogger logger, Exception exception);
}
