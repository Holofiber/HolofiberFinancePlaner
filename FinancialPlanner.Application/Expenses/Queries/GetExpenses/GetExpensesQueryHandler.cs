using FinancialPlanner.Application.Abstractions.Persistence;
using MediatR;

namespace FinancialPlanner.Application.Expenses.Queries.GetExpenses;

public sealed class GetExpensesQueryHandler(IExpenseRepository expenseRepository)
    : IRequestHandler<GetExpensesQuery, IReadOnlyCollection<ExpenseDto>>
{
    public async Task<IReadOnlyCollection<ExpenseDto>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var expenses = await expenseRepository.ListByUserIdAsync(request.UserId, cancellationToken);

        return expenses
            .OrderByDescending(e => e.OccurredAtUtc)
            .Select(e => new ExpenseDto(e.Id, e.Amount, e.Category, e.OccurredAtUtc, e.Note))
            .ToArray();
    }
}
