namespace Application.DTOs;

public sealed record CadastrarTalhaoDTO
{
    public required Guid UsuarioId { get; init; }
    public required Guid PropriedadeId { get; init; }
    public required string Nome { get; init; }
    public string? Descricao { get; init; }
    public required CoordenadasDTO Coordenadas { get; init; }
    public required int Area { get; init; }
}
