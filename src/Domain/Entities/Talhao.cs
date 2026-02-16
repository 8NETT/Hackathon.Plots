using Domain.Builders;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Talhao : BaseEntity
{
    public Guid PropriedadeId { get; internal set; }
    public string Nome { get; internal set; }
    public string? Descricao { get; internal set; }
    public Coordenadas Coordenadas { get; internal set; }
    public decimal Area { get; internal set; }
    public Propriedade Propriedade { get; internal set; }

    internal Talhao() { Nome = null!; Coordenadas = null!; Propriedade = null!; }

    public static TalhaoBuilder Novo => new TalhaoBuilder();

    public void Alterar(string nome, string? descricao, Coordenadas coordenadas, decimal area)
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentNullException(nameof(Nome));
        if (coordenadas is null)
            throw new ArgumentNullException(nameof(coordenadas));
        if (area <= 0M)
            throw new ArgumentOutOfRangeException(nameof(area));

        Nome = nome;
        Descricao = descricao;
        Coordenadas = coordenadas;
        Area = area;
    }
}
