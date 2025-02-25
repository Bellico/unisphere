using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Unisphere.Core.Presentation;

public partial class GrpcGlobalExceptionHandlerInterceptor(
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
            return await continuation(request, context);
        }
        catch (ValidationException validationException)
        {
            throw ProblemDetailHelper.CreateRpcException(validationException);
        }
        catch (Exception exception) when (exception is not RpcException)
        {
            LogExceptionError(logger, exception);

            throw ProblemDetailHelper.CreateRpcException(exception);
        }
    }

    [LoggerMessage(1, LogLevel.Error, "An unhandled exception has occurred")]
    static partial void LogExceptionError(ILogger logger, Exception exception);
}
