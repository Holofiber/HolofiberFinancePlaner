namespace FinancialPlanner.Application.Abstractions.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email);

    string GenerateRefreshToken();
}
