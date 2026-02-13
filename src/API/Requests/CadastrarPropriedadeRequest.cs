namespace API.Requests;

public sealed record CadastrarPropriedadeRequest
{
    public string? Nome { get; init; }
    public string? Descricao { get; init; }
    public EnderecoRequest? Endereco { get; init; }
}
