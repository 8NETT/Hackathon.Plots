namespace API.Requests;

public record CadastrarPropriedadeRequest
{
    [FromBody]
    public string? Nome { get; set; }

    [FromBody]
    public string? Descricao { get; set; }

    [FromBody]
    public EnderecoRequest? Endereco { get; set; }
}
