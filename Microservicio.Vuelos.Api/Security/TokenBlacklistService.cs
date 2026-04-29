using System.Collections.Concurrent;

namespace Microservicio.Vuelos.Api.Security;

public class TokenBlacklistService : ITokenBlacklistService
{
    private readonly ConcurrentDictionary<string, DateTimeOffset> _tokens = new();

    public void Blacklist(string token, DateTimeOffset expiresAtUtc)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        CleanupExpired();
        _tokens[token] = expiresAtUtc;
    }

    public bool IsBlacklisted(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        CleanupExpired();
        return _tokens.ContainsKey(token);
    }

    private void CleanupExpired()
    {
        var now = DateTimeOffset.UtcNow;

        foreach (var item in _tokens)
        {
            if (item.Value <= now)
                _tokens.TryRemove(item.Key, out _);
        }
    }
}
