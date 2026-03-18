using FinancialPlanner.Application.Abstractions.Caching;
using FinancialPlanner.Application.Abstractions.Persistence;
using MediatR;

namespace FinancialPlanner.Application.Expenses.Queries.GetExpenses;

public sealed class GetExpensesQueryHandler(
    IExpenseRepository expenseRepository,
    ICacheService cacheService)
    : IRequestHandler<GetExpensesQuery, IReadOnlyCollection<ExpenseDto>>
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public async Task<IReadOnlyCollection<ExpenseDto>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = ExpenseCacheKeys.UserExpenses(request.UserId);
        var cachedExpenses = await cacheService.GetAsync<IReadOnlyCollection<ExpenseDto>>(cacheKey, cancellationToken);
        if (cachedExpenses is not null)
        {
            return cachedExpenses;
        }

        var expenses = await expenseRepository.ListByUserIdAsync(request.UserId, cancellationToken);

        var result = expenses
            .OrderByDescending(e => e.OccurredAtUtc)
            .Select(e => new ExpenseDto(e.Id, e.Amount, e.Category, e.OccurredAtUtc, e.Note))
            .ToArray();

        await cacheService.SetAsync(cacheKey, result, CacheDuration, cancellationToken);

        return result;
    }
}
