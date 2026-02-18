using FluentValidation;
using System.Reflection;
using FinancialPlanner.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialPlanner.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    public static IServiceCollection AddInfrastructureViaReflection(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var infrastructureAssembly = Assembly.Load("FinancialPlanner.Infrastructure");
        var extensionType = infrastructureAssembly.GetType("FinancialPlanner.Infrastructure.DependencyInjection.ServiceCollectionExtensions")
            ?? throw new InvalidOperationException("Infrastructure DI extension type was not found.");

        var method = extensionType.GetMethod("AddInfrastructure", BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("AddInfrastructure method was not found.");

        method.Invoke(null, [services, configuration]);
        return services;
    }
}
