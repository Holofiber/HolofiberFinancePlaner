using FinancialPlanner.Domain.Common;

namespace FinancialPlanner.Domain.Entities;

public sealed class Expense : Entity
{
    private Expense()
    {
    }

    private Expense(Guid id, Guid userId, decimal amount, string category, DateTime occurredAtUtc, string? note)
    {
        Id = id;
        UserId = userId;
        SetAmount(amount);
        SetCategory(category);
        OccurredAtUtc = occurredAtUtc;
        Note = note?.Trim();
    }

    public Guid UserId { get; private set; }

    public decimal Amount { get; private set; }

    public string Category { get; private set; } = string.Empty;

    public DateTime OccurredAtUtc { get; private set; }

    public string? Note { get; private set; }

    public static Expense Create(Guid userId, decimal amount, string category, DateTime occurredAtUtc, string? note)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId must be provided.", nameof(userId));
        }

        return new Expense(Guid.NewGuid(), userId, amount, category, occurredAtUtc, note);
    }

    private void SetAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Expense amount must be greater than zero.");
        }

        Amount = amount;
    }

    private void SetCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Category is required.", nameof(category));
        }

        Category = category.Trim();
    }
}
