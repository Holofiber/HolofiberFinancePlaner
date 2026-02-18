using MediatR;

namespace FinancialPlanner.Application.Authentication.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

public sealed record LoginResponse(string AccessToken, string RefreshToken, Guid UserId);
