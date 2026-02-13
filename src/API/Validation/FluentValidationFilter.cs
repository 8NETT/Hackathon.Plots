namespace API.Validation;

internal class FluentValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        foreach (var argument in context.Arguments)
        {
            if (argument is null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = context.HttpContext.RequestServices.GetService(validatorType);

            if (validator is IValidator fluentValidator)
            {
                var result = await fluentValidator.ValidateAsync(new ValidationContext<object>(argument));

                if (!result.IsValid)
                    return Results.BadRequest(result.Errors);
            }
        }

        return await next(context);
    }
}
