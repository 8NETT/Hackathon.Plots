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

    internal Talhao() { Nome = null!; Coordenadas = null!; }

    public static TalhaoBuilder Novo => new TalhaoBuilder();    
}
