namespace Application.UseCases.Talhoes.ObterTalhoesDaPropriedade;

public sealed record ObterTalhoesDaPropriedadeDTO
{
    public required Guid PropriedadeId { get; init; }
    public required Guid UsuarioId { get; init; }
}
