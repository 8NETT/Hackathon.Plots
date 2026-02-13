namespace API.Requests;

public sealed record CadastrarPropriedadeRequest
{
    public string? Nome { get; init; }
    public string? Descricao { get; init; }
    public CadastrarEnderecoRequest? Endereco { get; init; }
}
