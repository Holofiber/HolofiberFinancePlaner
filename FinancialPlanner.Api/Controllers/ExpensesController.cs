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
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Create([FromBody] CreateExpenseCommand command, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var expenseId = await sender.Send(command with { UserId = userId }, cancellationToken);
        return Ok(new { expenseId });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
}
