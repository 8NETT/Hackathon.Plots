namespace Application.UseCases.Talhoes.ObterTalhao;

public sealed record ObterTalhaoDTO
{
    public required Guid Id { get; init; }
    public required Guid UsuarioId { get; init; }
}
