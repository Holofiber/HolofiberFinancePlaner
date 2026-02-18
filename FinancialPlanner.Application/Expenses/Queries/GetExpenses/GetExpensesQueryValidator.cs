using FluentValidation;

namespace FinancialPlanner.Application.Expenses.Queries.GetExpenses;

public sealed class GetExpensesQueryValidator : AbstractValidator<GetExpensesQuery>
{
    public GetExpensesQueryValidator()
    {
        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
    }
}
