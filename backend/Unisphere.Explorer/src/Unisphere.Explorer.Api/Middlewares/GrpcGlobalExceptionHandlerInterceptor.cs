using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Unisphere.Explorer.Api.Middlewares;

internal partial class GrpcGlobalExceptionHandlerInterceptor(
    IHostEnvironment env,
    ILogger<GrpcGlobalExceptionHandlerInterceptor> logger)
    : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await base.UnaryServerHandler(request, context, continuation);
        }
        catch (ValidationException validationException)
        {
            var validationErrors = validationException.Errors
                               .GroupBy(e => e.PropertyName)
                               .ToDictionary(f => f.Key, f => f.ToList());

            throw new RpcException(new Status(StatusCode.InvalidArgument, validationException.ToString()));
        }
        catch (Exception exception) when (exception is not RpcException)
        {
            LogExceptionError(logger, exception);

            throw new RpcException(new Status(StatusCode.Internal, exception.ToString()));
        }
    }

    [LoggerMessage(1, LogLevel.Error, "An unhandled exception has occurred")]
    static partial void LogExceptionError(ILogger logger, Exception exception);
}
