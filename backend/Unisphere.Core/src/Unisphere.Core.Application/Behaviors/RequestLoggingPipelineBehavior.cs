using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Unisphere.Core.Application.Behaviors;

public sealed partial class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        LogProcessingRequest(logger, requestName);

        TResponse result = await next();

        if (result is IErrorOr resultValue)
        {
            if (!resultValue.IsError)
            {
                LogCompletedRequest(logger, requestName);
            }
            else
            {
                LogFailedRequest(logger, requestName);
            }
        }
        else
        {
            LogCompletedRequest(logger, requestName);
        }

        return result;
    }

    [LoggerMessage(1, LogLevel.Information, "Processing request {requestName}")]
    static partial void LogProcessingRequest(ILogger logger, string requestName);

    [LoggerMessage(2, LogLevel.Information, "Completed request {requestName}")]
    static partial void LogCompletedRequest(ILogger logger, string requestName);

    [LoggerMessage(3, LogLevel.Error, "Failed request {requestName}")]
    static partial void LogFailedRequest(ILogger logger, string requestName);
}
