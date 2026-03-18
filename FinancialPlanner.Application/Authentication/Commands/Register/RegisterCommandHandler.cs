using FinancialPlanner.Application.Abstractions.Authentication;
using FinancialPlanner.Application.Abstractions.Persistence;
using FinancialPlanner.Domain.Entities;
using FinancialPlanner.Domain.ValueObjects;
using MediatR;

namespace FinancialPlanner.Application.Authentication.Commands.Register;

public sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = Email.Create(request.Email).Value;
        var existingUser = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var user = User.Create(Email.Create(request.Email), passwordHasher.Hash(request.Password));
        await userRepository.AddAsync(user, cancellationToken);

        var refreshTokenValue = jwtTokenGenerator.GenerateRefreshToken();
        var refreshToken = RefreshToken.Create(user.Id, refreshTokenValue, DateTime.UtcNow.AddDays(7));
        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse(
            jwtTokenGenerator.GenerateToken(user.Id, user.Email.Value),
            refreshTokenValue,
            user.Id);
    }
}
