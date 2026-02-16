namespace Application.UseCases.Propriedades.RemoverPropriedade;

public sealed record RemoverPropriedadeDTO
{
    public required Guid Id { get; init; }
    public required Guid UsuarioId { get; init; }
}
