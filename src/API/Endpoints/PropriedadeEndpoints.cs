using API.Mapping;
using API.Requests;
using API.Validation;
using Application.UseCases.Propriedades;
using Ardalis.Result;

namespace API.Endpoints;

internal static class PropriedadeEndpoints
{
    public static IEndpointRouteBuilder MapPropriedadeEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/propriedades")
            .WithTags("Propriedades")
            .AddEndpointFilter<FluentValidationFilter>();

        group.MapPost("/", async (
            CadastrarPropriedadeRequest request,
            ICadastrarPropriedadeUseCase useCase,
            HttpContext context,
            CancellationToken ct) =>
        {
            var userIdClaim = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userIdClaim is null)
                return Results.Unauthorized();
            if (!Guid.TryParse(userIdClaim, out var authenticatedUserId))
                return Results.Unauthorized();

            var result = await useCase.HandleAsync(request.ToApplicationDTO(authenticatedUserId), ct);

            if (result.IsInvalid())
                return Results.BadRequest(result.ValidationErrors);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Created($"/propriedades/{result.Value.Id}", result.Value);
        })
            .RequireAuthorization()
            .WithSummary("Cadastra uma nova propriedade")
            .Produces<CadastrarPropriedadeRequest>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Accepts<CadastrarPropriedadeRequest>("application/json");

        return group;
    }
}
