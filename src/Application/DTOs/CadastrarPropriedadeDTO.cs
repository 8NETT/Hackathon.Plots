namespace Application.DTOs;

public sealed record CadastrarPropriedadeDTO
{
    public required Guid ProprietarioId { get; init; }
    public required string Nome { get; init; }
    public string? Descricao { get; init; }
    public EnderecoDTO? Endereco { get; init; }
}
