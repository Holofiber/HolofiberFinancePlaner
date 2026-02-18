using MediatR;

namespace FinancialPlanner.Application.Expenses.Queries.GetExpenses;

public sealed record GetExpensesQuery(Guid UserId) : IRequest<IReadOnlyCollection<ExpenseDto>>;

public sealed record ExpenseDto(Guid Id, decimal Amount, string Category, DateTime OccurredAtUtc, string? Note);
