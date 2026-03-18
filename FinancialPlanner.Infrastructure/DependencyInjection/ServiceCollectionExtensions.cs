using System.Text;
using FinancialPlanner.Application.Abstractions.Authentication;
using FinancialPlanner.Application.Abstractions.Caching;
using FinancialPlanner.Application.Abstractions.Persistence;
using FinancialPlanner.Infrastructure.Authentication;
using FinancialPlanner.Infrastructure.Caching;
using FinancialPlanner.Infrastructure.Persistence;
using FinancialPlanner.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FinancialPlanner.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string was not found.");
        var redisConnectionString = configuration["Redis:ConnectionString"] ?? "localhost:6379";
        var redisInstanceName = configuration["Redis:InstanceName"] ?? "FinancialPlanner:";

        services.AddDbContext<FinancialPlannerDbContext>(options => options.UseNpgsql(connectionString));
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = redisInstanceName;
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICacheService, RedisCacheService>();

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

        var secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT secret is not configured.");
        var issuer = configuration["Jwt:Issuer"] ?? "FinancialPlanner";
        var audience = configuration["Jwt:Audience"] ?? "FinancialPlanner.Api";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
            });

        services.AddAuthorization();

        return services;
    }
}
