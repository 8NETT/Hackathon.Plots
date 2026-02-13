using API.Mapping;
using API.Requests;
using API.Security;
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
            .AddEndpointFilter<FluentValidationFilter>()
            .RequireAuthorization();

        group.MapGet("/{id:guid}", async (
            Guid id,
            IObterPropriedadeUseCase useCase,
            HttpContext context,
            CancellationToken ct) =>
        {
            var authenticatedUserId = context.User.GetAuthenticatedUserId();

            if (authenticatedUserId is null)
                return Results.Unauthorized();

            var result = await useCase.HandleAsync(id, ct);

            if (result.IsNotFound())
                return Results.NotFound();
            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);
            if (result.Value.ProprietarioId != authenticatedUserId)
                return Results.Unauthorized();

            return Results.Ok(result.Value);
        });

        group.MapPost("/", async (
            CadastrarPropriedadeRequest request,
            ICadastrarPropriedadeUseCase useCase,
            HttpContext context,
            CancellationToken ct) =>
        {
            var authenticatedUserId = context.User.GetAuthenticatedUserId();

            if (authenticatedUserId is null)
                return Results.Unauthorized();

            var result = await useCase.HandleAsync(request.ToApplicationDTO(authenticatedUserId.Value), ct);

            if (result.IsInvalid())
                return Results.BadRequest(result.ValidationErrors);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Created($"/propriedades/{result.Value.Id}", result.Value);
        })
            .WithSummary("Cadastra uma nova propriedade")
            .Produces<CadastrarPropriedadeRequest>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Accepts<CadastrarPropriedadeRequest>("application/json");

        return group;
    }
}
