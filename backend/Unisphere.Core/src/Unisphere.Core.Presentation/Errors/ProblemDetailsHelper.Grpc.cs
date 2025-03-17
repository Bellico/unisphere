using System.Diagnostics;
using System.Text;
using System.Text.Json;
using ErrorOr;
using FluentValidation;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Unisphere.Core.Presentation.Errors;

public static partial class ProblemDetailHelper
{
    public static ProblemDetails CreateProblemDetails(HttpContext context, RpcException exception, IHostEnvironment env)
    {
        var errors = exception.Trailers.GetValueBytes("errors-bin");

        var problemDetails = new ProblemDetails
        {
            Status = MapGrpcToHttpStatus(exception.StatusCode),
            Type = ProblemTypes[MapGrpcToHttpStatus(exception.StatusCode)],
            Title = InternalServerErrorTitle,
            Extensions = { },
        };

        if (errors is not null)
        {
            var validationErrors = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(Encoding.UTF8.GetString(errors));
            problemDetails.Title = exception.Status.Detail;
            problemDetails.Extensions["errors"] = validationErrors;

            return problemDetails;
        }

        if (!env.IsDevelopment())
        {
            return problemDetails;
        }

        problemDetails.Detail = exception.Status.Detail;
        problemDetails.Extensions["traceId"] = Activity.Current?.Id;
        problemDetails.Extensions["requestId"] = context.TraceIdentifier;
        problemDetails.Extensions["data"] = exception.Data;

        return problemDetails;
    }

    public static RpcException CreateRpcException(Exception exception)
    {
        return new RpcException(
           new Status(MapHttpStatusToGrpc(StatusCodes.Status500InternalServerError), exception.ToString()));
    }

    public static RpcException CreateRpcException(ValidationException validationException)
    {
        var validationErrors = validationException.Errors
                 .GroupBy(e => e.PropertyName)
                 .ToDictionary(f => f.Key, f => f.Select(u => u.ErrorMessage).ToList());

        var errorMessageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(validationErrors));
        var metadata = new Metadata
        {
            { "errors-bin", errorMessageBytes },
        };

        return new RpcException(
                new Status(MapHttpStatusToGrpc(StatusCodes.Status400BadRequest), BadRequestErrorTitle),
                metadata);
    }

    public static RpcException RpcProblem(IList<Error> errors)
    {
        var problems = errors
            .GroupBy(e => e.Code)
            .ToDictionary(f => f.Key, f => f.Select(u => u.Description).ToList());

        var errorMessageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(problems));
        var metadata = new Metadata
        {
            { "errors-bin", errorMessageBytes },
        };

        var statusCode = MapHttpStatusToGrpc(GetStatusCode(errors[0].Type));

        return new RpcException(
            new Status(statusCode, errors[0].Code),
            metadata);
    }

    private static int MapGrpcToHttpStatus(StatusCode statusCode) => statusCode switch
    {
        StatusCode.NotFound => StatusCodes.Status404NotFound,
        StatusCode.InvalidArgument => StatusCodes.Status400BadRequest,
        StatusCode.PermissionDenied => StatusCodes.Status403Forbidden,
        StatusCode.Unauthenticated => StatusCodes.Status401Unauthorized,
        StatusCode.Internal => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError,
    };

    private static StatusCode MapHttpStatusToGrpc(int statusCode) => statusCode switch
    {
        StatusCodes.Status404NotFound => StatusCode.NotFound,
        StatusCodes.Status400BadRequest => StatusCode.InvalidArgument,
        StatusCodes.Status403Forbidden => StatusCode.PermissionDenied,
        StatusCodes.Status401Unauthorized => StatusCode.Unauthenticated,
        StatusCodes.Status500InternalServerError => StatusCode.Internal,
        _ => StatusCode.Internal,
    };
}
