using MediatR;

namespace FinancialPlanner.Application.Authentication.Commands.Register;

public sealed record RegisterCommand(string Email, string Password) : IRequest<AuthResponse>;

public sealed record AuthResponse(string AccessToken, string RefreshToken, Guid UserId);
