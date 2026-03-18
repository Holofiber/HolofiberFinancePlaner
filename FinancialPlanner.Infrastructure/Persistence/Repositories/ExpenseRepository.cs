using FinancialPlanner.Application.Abstractions.Persistence;
using FinancialPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanner.Infrastructure.Persistence.Repositories;

public sealed class ExpenseRepository(FinancialPlannerDbContext dbContext) : IExpenseRepository
{
    public async Task AddAsync(Expense expense, CancellationToken cancellationToken)
    {
        await dbContext.Expenses.AddAsync(expense, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Expense>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.Expenses.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
    }
}
