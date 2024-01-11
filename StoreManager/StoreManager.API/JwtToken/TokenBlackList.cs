namespace StoreManager.API.JwtToken;

public static class TokenBlackList
{
    private static readonly HashSet<string> _revokedTokens = new HashSet<string>();

    public static void RevokeToken(string token) => _revokedTokens.Add(token);

    public static bool IsTokenRevoked(string token) => _revokedTokens.Contains(token);
}
