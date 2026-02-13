using FluentValidation;

namespace API.Configurations;

internal static class ValidationConfiguration
{
    public static IServiceCollection AddValidation(this IServiceCollection services) =>
        services.AddValidatorsFromAssemblyContaining<Program>();
}
