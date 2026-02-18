using FinancialPlanner.Application.Abstractions.Persistence;

namespace FinancialPlanner.Infrastructure.Persistence;

public sealed class UnitOfWork(FinancialPlannerDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
