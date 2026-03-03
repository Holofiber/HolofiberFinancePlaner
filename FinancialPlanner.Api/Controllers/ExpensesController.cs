using System.Security.Claims;
using FinancialPlanner.Application.Expenses.Commands.CreateExpense;
using FinancialPlanner.Application.Expenses.Queries.GetExpenses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPlanner.Api.Controllers;

[ApiController]
[Route("api/expenses")]
[Authorize]
public sealed class ExpensesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Creates a new expense for the authenticated user.
    /// </summary>
    /// <param name="command">Expense data to create.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The identifier of the created expense.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateExpenseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Create([FromBody] CreateExpenseCommand command, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var expenseId = await sender.Send(command with { UserId = userId }, cancellationToken);
        return Ok(new CreateExpenseResponse(expenseId));
    }

    /// <summary>
    /// Returns all expenses for the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A list of the user's expenses.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ExpenseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await sender.Send(new GetExpensesQuery(userId), cancellationToken);
        return Ok(result);
    }

    private Guid GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue(ClaimTypes.Name)
                    ?? throw new UnauthorizedAccessException("User identifier claim was not found.");

        return Guid.Parse(value);
    }

    public sealed record CreateExpenseResponse(Guid ExpenseId);
}
