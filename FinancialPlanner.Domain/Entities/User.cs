using FinancialPlanner.Domain.Common;
using FinancialPlanner.Domain.ValueObjects;

namespace FinancialPlanner.Domain.Entities;

public sealed class User : Entity
{
    private readonly List<RefreshToken> _refreshTokens = [];

    private User()
    {
    }

    private User(Guid id, Email email, string passwordHash)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
    }

    public Email Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = string.Empty;

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

    public static User Create(Email email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
        }

        return new User(Guid.NewGuid(), email, passwordHash);
    }

    public void AddRefreshToken(string token, DateTime expiresAt)
    {
        _refreshTokens.Add(RefreshToken.Create(Id, token, expiresAt));
    }
}
