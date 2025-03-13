using ErrorOr;

namespace Unisphere.Core.Presentation;

public static class ErrorOrHelper
{
    public static TNextValue OnSuccess<TValue, TNextValue>(this ErrorOr<TValue> result, Func<TValue, TNextValue> onValue) where TValue : new()
    {
        return result.Match(
            onValue,
            errors => throw ProblemDetailHelper.RpcProblem(errors));
    }
}
