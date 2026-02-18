using FinancialPlanner.Application.Abstractions.Authentication;
using FinancialPlanner.Application.Abstractions.Persistence;
using FinancialPlanner.Domain.Entities;
using FinancialPlanner.Domain.ValueObjects;
using MediatR;

namespace FinancialPlanner.Application.Authentication.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email).Value;
        var user = await userRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new InvalidOperationException("Invalid credentials.");

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        var refreshTokenValue = jwtTokenGenerator.GenerateRefreshToken();
        var refreshToken = RefreshToken.Create(user.Id, refreshTokenValue, DateTime.UtcNow.AddDays(7));
        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse(
            jwtTokenGenerator.GenerateToken(user.Id, user.Email.Value),
            refreshTokenValue,
            user.Id);
    }
}
