using FinancialPlanner.Domain.Entities;

namespace FinancialPlanner.Application.Abstractions.Persistence;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}
