using System.Collections.Immutable;

namespace Unisphere.Core.Common.Constants;

public static class UnisphereConstants
{
    public static class Scopes
    {
        public const string IdentityApi = "identity-api";

        public const string ExplorerApi = "explorer-api";

        public static readonly ImmutableArray<string> AuthorizedScopes =
        [
            IdentityApi,
            ExplorerApi,
        ];
    }

    public static class PoliciesNames
    {
        public const string GatewayPolicy = "gateway-policy";

        public const string IdentityPolicy = "identity-policy";

        public const string ExplorerPolicy = "explorer-policy";
    }
}
