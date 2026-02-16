namespace API.Requests;

public record CadastrarTalhaoRequest
{
    [FromBody]
    public Guid? PropriedadeId { get; set; }

    [FromBody]
    public string? Nome { get; set; }

    [FromBody]
    public string? Descricao { get; set; }

    [FromBody]
    public CoordenadasRequest? Coordenadas { get; set; }

    [FromBody]
    public decimal? Area { get; set; }
}
