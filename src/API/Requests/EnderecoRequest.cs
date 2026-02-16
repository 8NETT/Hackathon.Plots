namespace API.Requests;

public sealed record EnderecoRequest
{
    [FromBody]
    public string? Logradouro { get; init; }

    [FromBody]
    public string? Numero { get; init; }

    [FromBody]
    public string? Complemento { get; init; }

    [FromBody]
    public string? Bairro { get; init; }

    [FromBody]
    public string? Cidade { get; init; }

    [FromBody]
    public string? UF { get; init; }

    [FromBody]
    public string? CEP { get; init; }
}