namespace Application.Extensions;

internal static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty?> SetOptionalValidator<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder,
        IValidator<TProperty> validator,
        params string[] ruleSets)
        where TProperty : class
    {
        return ruleBuilder.SetValidator((IValidator<TProperty?>)validator, ruleSets);
    }
}
