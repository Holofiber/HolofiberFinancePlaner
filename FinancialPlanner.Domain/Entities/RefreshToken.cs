using FinancialPlanner.Domain.Common;

namespace FinancialPlanner.Domain.Entities;

public sealed class RefreshToken : Entity
{
    private RefreshToken()
    {
    }

    private RefreshToken(Guid id, Guid userId, string token, DateTime expiresAtUtc)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiresAtUtc = expiresAtUtc;
    }

    public Guid UserId { get; private set; }

    public string Token { get; private set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; private set; }

    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAtUtc;

    public static RefreshToken Create(Guid userId, string token, DateTime expiresAtUtc)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId must be provided.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Refresh token must be provided.", nameof(token));
        }

        if (expiresAtUtc <= DateTime.UtcNow)
        {
            throw new ArgumentOutOfRangeException(nameof(expiresAtUtc), "Refresh token expiry must be in the future.");
        }

        return new RefreshToken(Guid.NewGuid(), userId, token, expiresAtUtc);
    }
}
