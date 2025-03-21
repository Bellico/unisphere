using ErrorOr;
using Unisphere.Core.Presentation.Errors;

namespace Unisphere.Core.Presentation.Extensions;

public static class ErrorOrExtensions
{
    public static TNextValue OnSuccess<TValue, TNextValue>(this ErrorOr<TValue> result, Func<TValue, TNextValue> onValue) where TValue : new()
    {
        return result.Match(
            onValue,
            errors => throw ProblemDetailHelper.RpcProblem(errors));
    }
}
