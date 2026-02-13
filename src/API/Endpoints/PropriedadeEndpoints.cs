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

            var result = await useCase.HandleAsync(new() { Id = id, UsuarioId = authenticatedUserId.Value }, ct);

            if (result.IsNotFound()) return Results.NotFound();
            if (result.IsForbidden()) return Results.Forbid();
            if (!result.IsSuccess) return Results.Problem();

            return Results.Ok(result.Value);
        })
            .WithSummary("Obtém os dados de uma propriedade")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

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
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Accepts<CadastrarPropriedadeRequest>("application/json");

        group.MapPut("/{id:guid}", async (
            Guid id,
            AlterarPropriedadeRequest request,
            IAlterarPropriedadeUseCase useCase,
            HttpContext context,
            CancellationToken ct) =>
        {
            var authenticatedUserId = context.User.GetAuthenticatedUserId();

            if (authenticatedUserId is null)
                return Results.Unauthorized();

            var result = await useCase.HandleAsync(request.ToApplicationDTO(id, authenticatedUserId.Value), ct);

            if (result.IsNotFound())
                return Results.NotFound();
            if (result.IsForbidden())
                return Results.Unauthorized();
            if (result.IsInvalid())
                return Results.BadRequest(result.ValidationErrors);
            if (!result.IsSuccess)
                return Results.BadRequest(result.Errors);

            return Results.Ok(result.Value);
        })
            .WithSummary("Altera os dados de uma propriedade existente")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<AlterarPropriedadeRequest>("application/json");

        return group;
    }
}
