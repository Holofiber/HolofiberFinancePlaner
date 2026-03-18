namespace FinancialPlanner.Application.Expenses;

public static class ExpenseCacheKeys
{
    public static string UserExpenses(Guid userId) => $"expenses:user:{userId:N}";
}
