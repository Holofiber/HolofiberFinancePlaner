namespace FinancialPlanner.Contracts.Expenses;

public sealed record ExpenseResponse(Guid Id, decimal Amount, string Category, DateTime OccurredAtUtc, string? Note);
