namespace Application.DTOs;

public sealed record TalhaoDTO
{
    public required Guid Id { get; init; }
    public required Guid PropriedadeId { get; init; }
    public required string Nome { get; init; }
    public string? Descricao { get; init; }
    public required CoordenadasDTO Coordenadas { get; init; }
    public required decimal Area { get; init; }
}
