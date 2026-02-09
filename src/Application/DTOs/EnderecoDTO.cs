namespace Application.DTOs;

public sealed record EnderecoDTO
{
    public required string Logradouro { get; init; }
    public required string Numero { get; init; }
    public string? Complemento { get; init; }
    public string? Bairro { get; init; }
    public required string Cidade { get; init; }
    public required string UF { get; init; }
    public string? CEP { get; init; }
}
