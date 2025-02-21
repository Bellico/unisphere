using ErrorOr;
using Grpc.Core;

namespace Unisphere.Explorer.Api;

internal static class GrpcCustomResults
{
    public static RpcException Problem(IList<Error> errors)
    {
        var error = errors.FirstOrDefault();

        return new RpcException(
            new Status(GetStatusCode(error.Type), error.Description),
            new Metadata
            {
              { error.Code, error.Description },
            });
    }

    static StatusCode GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCode.InvalidArgument,
            ErrorType.NotFound => StatusCode.NotFound,
            ErrorType.Conflict => StatusCode.AlreadyExists,
            ErrorType.Forbidden => StatusCode.PermissionDenied,
            ErrorType.Unauthorized => StatusCode.Unauthenticated,
            _ => StatusCode.Internal
        };
}
