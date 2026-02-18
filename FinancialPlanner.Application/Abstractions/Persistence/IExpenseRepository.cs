using FinancialPlanner.Domain.Entities;

namespace FinancialPlanner.Application.Abstractions.Persistence;

public interface IExpenseRepository
{
    Task AddAsync(Expense expense, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Expense>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
