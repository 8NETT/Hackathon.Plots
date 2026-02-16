using Application.DTOs;

namespace Application.UseCases.Propriedades.AlterarPropriedade;

public sealed record AlterarPropriedadeDTO
{
    public required Guid Id { get; init; }
    public required Guid UsuarioId { get; init; }
    public required string Nome { get; init; }
    public string? Descricao { get; init; }
    public EnderecoDTO? Endereco { get; init; }
}
