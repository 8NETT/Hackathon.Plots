using Application.DTOs;

namespace Application.UseCases.Talhoes.AlterarTalhao;

public sealed record AlterarTalhaoDTO
{
    public required Guid Id { get; init; }
    public required Guid UsuarioId { get; init; }
    public required string Nome { get; init; }
    public string? Descricao { get; init; }
    public required CoordenadasDTO Coordenadas { get; init; }
    public required decimal Area { get; init; }
}
