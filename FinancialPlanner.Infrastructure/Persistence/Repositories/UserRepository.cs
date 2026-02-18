using FinancialPlanner.Application.Abstractions.Persistence;
using FinancialPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanner.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(FinancialPlannerDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await dbContext.Users.FirstOrDefaultAsync(x => x.Email.Value == email, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
    }
}
