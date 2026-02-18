using FinancialPlanner.Application.Abstractions.Persistence;
using FinancialPlanner.Application.Common.Exceptions;
using FinancialPlanner.Domain.Entities;
using MediatR;

namespace FinancialPlanner.Application.Expenses.Commands.CreateExpense;

public sealed class CreateExpenseCommandHandler(
    IUserRepository userRepository,
    IExpenseRepository expenseRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateExpenseCommand, Guid>
{
    public async Task<Guid> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        var expense = Expense.Create(
            request.UserId,
            request.Amount,
            request.Category,
            request.OccurredAtUtc == default ? DateTime.UtcNow : request.OccurredAtUtc,
            request.Note);

        await expenseRepository.AddAsync(expense, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return expense.Id;
    }
}
