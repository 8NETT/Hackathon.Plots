namespace Application.UseCases.Talhoes.RemoverTalhao;

public sealed record RemoverTalhaoDTO
{
    public required Guid Id { get; init; }
    public required Guid UsuarioId { get; init; }
}
