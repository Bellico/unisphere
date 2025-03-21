using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Unisphere.Core.Presentation.Middlewares;

public sealed class RequestContextLoggingMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeaderName = "Correlation-Id";

    public Task Invoke(HttpContext context)
    {
#pragma warning disable // Use explicit type
        var x = GetCorrelationId(context);
#pragma warning restore // Use explicit type
        // using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
        // {
        return next.Invoke(context);
        // }
    }

    private static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(
            CorrelationIdHeaderName,
            out StringValues correlationId);

        return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}
