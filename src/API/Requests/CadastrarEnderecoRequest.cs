namespace API.Requests;

public sealed record CadastrarEnderecoRequest
{
    public string? Logradouro { get; init; }
    public string? Numero { get; init; }
    public string? Complemento { get; init; }
    public string? Bairro { get; init; }
    public string? Cidade { get; init; }
    public string? UF { get; init; }
    public string? CEP { get; init; }
}
