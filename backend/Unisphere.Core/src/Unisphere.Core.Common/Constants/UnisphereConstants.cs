using System.Collections.Immutable;

namespace Unisphere.Core.Common.Constants;

public static class UnisphereConstants
{
    public static class Scopes
    {
        public const string ExplorerApi = "explorer-api";

        public static readonly ImmutableArray<string> AuthorizedScopes =
        [
            ExplorerApi,
        ];
    }

    public static class PoliciesNames
    {
        public const string ExplorerPolicy = "explorer-policy";
    }
}
