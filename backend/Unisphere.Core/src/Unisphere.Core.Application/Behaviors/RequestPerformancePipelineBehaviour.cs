using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Unisphere.Core.Application.Behaviors;

public partial class RequestPerformancePipelineBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public ILogger<RequestPerformancePipelineBehaviour<TRequest, TResponse>> Logger { get; }

    private readonly Stopwatch _timer;

    public RequestPerformancePipelineBehaviour(ILogger<RequestPerformancePipelineBehaviour<TRequest, TResponse>> logger)
    {
        _timer = new Stopwatch();
        Logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;

            LogLongRunningRequest(Logger, requestName, elapsedMilliseconds);
        }

        return response;
    }

    [LoggerMessage(1, LogLevel.Warning, "Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds)")]
    static partial void LogLongRunningRequest(ILogger logger, string requestName, long elapsedMilliseconds);
}
