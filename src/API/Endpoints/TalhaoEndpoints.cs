using API.Mapping;
using API.Requests;
using API.Security;
using API.Validation;
using Application.UseCases.Talhoes.AlterarTalhao;
using Application.UseCases.Talhoes.CadastrarTalhao;
using Application.UseCases.Talhoes.ObterTalhao;
using Application.UseCases.Talhoes.ObterTalhoesDaPropriedade;
using Application.UseCases.Talhoes.ObterTalhoesDoProprietario;
using Application.UseCases.Talhoes.RemoverTalhao;

namespace API.Endpoints;

internal static class TalhaoEndpoints
{
    public static IEndpointRouteBuilder MapTalhaoEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/talhao")
            .WithTags("Talhao")
            .AddEndpointFilter<FluentValidationFilter>()
            .RequireAuthorization();

        group.MapGet("/", async (
            IObterTalhoesDoProprietarioUseCase useCase,
            ICurrentUser user,
            CancellationToken ct) =>
        {
            var result = await useCase.HandleAsync(user.Id, ct);
            return Results.Ok(result.Value);
        })
            .WithSummary("Obtém os dados de talhões do usuário")
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", async (
            Guid id,
            IObterTalhaoUseCase useCase,
            ICurrentUser user,
            CancellationToken ct) =>
        {
            var dto = new ObterTalhaoDTO { Id = id, UsuarioId = user.Id };
            var result = await useCase.HandleAsync(dto, ct);

            if (result.IsNotFound())
                return Results.NotFound();
            if (result.IsForbidden())
                return Results.Forbid();

            return Results.Ok(result.Value);
        })
            .WithSummary("Obtém os dados de um talhão")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapGet("/propriedade/{propriedadeId:guid}", async (
            Guid propriedadeId,
            IObterTalhoesDaPropriedadeUseCase useCase,
            ICurrentUser user,
            CancellationToken ct) =>
        {
            var dto = new ObterTalhoesDaPropriedadeDTO { PropriedadeId = propriedadeId, UsuarioId = user.Id };
            var result = await useCase.HandleAsync(dto, ct);

            if (result.IsForbidden())
                return Results.Forbid();

            return Results.Ok(result.Value);
        })
            .WithSummary("Obtém os dados de talhões de uma propriedade")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapPost("/", async (
            CadastrarTalhaoRequest request,
            ICadastrarTalhaoUseCase useCase,
            ICurrentUser user,
            CancellationToken ct) =>
        {
            var dto = request.ToApplicationDTO(user.Id);
            var result = await useCase.HandleAsync(dto, ct);

            if (result.IsInvalid())
                return Results.BadRequest(result.ValidationErrors);
            if (result.IsNotFound())
                return Results.NotFound();
            if (result.IsForbidden())
                return Results.Forbid();

            return Results.Created($"/talhao/{result.Value.Id}", result.Value);
        })
            .WithSummary("Cadastra um novo talhão")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden)
            .Accepts<CadastrarTalhaoRequest>("application/json");

        group.MapPut("/{id:guid}", async (
            Guid id,
            AlterarTalhaoRequest request,
            IAlterarTalhaoUseCase useCase,
            ICurrentUser user,
            CancellationToken ct) =>
        {
            var dto = request.ToApplicationDTO(id, user.Id);
            var result = await useCase.HandleAsync(dto, ct);

            if (result.IsInvalid())
                return Results.BadRequest(result.ValidationErrors);
            if (result.IsNotFound())
                return Results.NotFound();
            if (result.IsForbidden())
                return Results.Forbid();

            return Results.Ok(result.Value);
        })
            .WithSummary("Altera os dados de um talhão")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<AlterarTalhaoRequest>("application/json");

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IRemoverTalhaoUseCase useCase,
            ICurrentUser user,
            CancellationToken ct) =>
        {
            var dto = new RemoverTalhaoDTO { Id = id, UsuarioId = user.Id };
            var result = await useCase.HandleAsync(dto, ct);

            if (result.IsNotFound())
                return Results.NotFound();
            if (result.IsForbidden())
                return Results.Forbid();

            return Results.Ok();
        })
            .WithSummary("Remove um talhão")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden);

        return group;
    }
}
