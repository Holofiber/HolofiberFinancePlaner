using FinancialPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanner.Infrastructure.Persistence;

public sealed class FinancialPlannerDbContext(DbContextOptions<FinancialPlannerDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Expense> Expenses => Set<Expense>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinancialPlannerDbContext).Assembly);
    }
}
