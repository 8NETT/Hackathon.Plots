using Application.DTOs;

namespace Application.UseCases.Propriedades.CadastrarPropriedade;

public sealed record CadastrarPropriedadeDTO
{
    public required Guid UsuarioId { get; init; }
    public required string Nome { get; init; }
    public string? Descricao { get; init; }
    public EnderecoDTO? Endereco { get; init; }
}
