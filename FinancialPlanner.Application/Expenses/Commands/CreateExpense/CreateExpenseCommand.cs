using MediatR;

namespace FinancialPlanner.Application.Expenses.Commands.CreateExpense;

public sealed record CreateExpenseCommand(Guid UserId, decimal Amount, string Category, DateTime OccurredAtUtc, string? Note)
    : IRequest<Guid>;
