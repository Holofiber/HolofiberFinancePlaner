namespace FinancialPlanner.Contracts.Expenses;

public sealed record CreateExpenseRequest(decimal Amount, string Category, DateTime OccurredAtUtc, string? Note);
