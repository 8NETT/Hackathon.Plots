namespace Application.DTOs;

public sealed record PropriedadeDTO
{
    public required Guid Id { get; init; }
    public required Guid ProprietarioId { get; init; }
    public required string Nome { get; init; }
    public string? Descricao { get; init; }
    public EnderecoDTO? Endereco { get; init; }
}
