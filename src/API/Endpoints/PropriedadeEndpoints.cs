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
            ICurrentUser user,
            CancellationToken ct) =>
        {
            var result = await useCase.HandleAsync(new() { Id = id, UsuarioId = user.Id }, ct);

            if (result.IsNotFound()) 
                return Results.NotFound();
            if (result.IsForbidden()) 
                return Results.Forbid();

            return Results.Ok(result.Value);
        })
            .WithSummary("Obtém os dados de uma propriedade")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("/", async (
            CadastrarPropriedadeRequest request,
            ICadastrarPropriedadeUseCase useCase,
            ICurrentUser user,
            CancellationToken ct) =>
        {
            var result = await useCase.HandleAsync(request.ToApplicationDTO(user.Id), ct);

            if (result.IsInvalid())
                return Results.BadRequest(result.ValidationErrors);

            return Results.Created($"/propriedades/{result.Value.Id}", result.Value);
        })
            .WithSummary("Cadastra uma nova propriedade")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Accepts<CadastrarPropriedadeRequest>("application/json");

        group.MapPut("/{id:guid}", async (
            Guid id,
            AlterarPropriedadeRequest request,
            IAlterarPropriedadeUseCase useCase,
            ICurrentUser user,
            CancellationToken ct) =>
        {
            var result = await useCase.HandleAsync(request.ToApplicationDTO(id, user.Id), ct);

            if (result.IsNotFound())
                return Results.NotFound();
            if (result.IsForbidden())
                return Results.Forbid();
            if (result.IsInvalid())
                return Results.BadRequest(result.ValidationErrors);

            return Results.Ok(result.Value);
        })
            .WithSummary("Altera os dados de uma propriedade existente")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<AlterarPropriedadeRequest>("application/json");

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IRemoverProrpriedadeUseCase usecase,
            ICurrentUser user,
            CancellationToken ct) =>
        {
            var result = await usecase.HandleAsync(new() { Id = id, UsuarioId = user.Id }, ct);

            if (result.IsNotFound())
                return Results.NotFound();
            if (result.IsForbidden())
                return Results.Forbid();

            return Results.Ok();
        })
            .WithSummary("Remove uma propriedade existente")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }
}
