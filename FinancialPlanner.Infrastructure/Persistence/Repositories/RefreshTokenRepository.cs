using FinancialPlanner.Application.Abstractions.Persistence;
using FinancialPlanner.Domain.Entities;

namespace FinancialPlanner.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository(FinancialPlannerDbContext dbContext) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }
}
