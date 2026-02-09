using Domain.Builders;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Propriedade : BaseEntity
{
    private List<Talhao> _talhoes = new();

    public Guid ProprietarioId { get; internal set; }
    public string Nome { get; internal set; }
    public string? Descricao { get; internal set; }
    public Endereco? Endereco { get; internal set; }
    public IReadOnlyCollection<Talhao> Talhoes => _talhoes.AsReadOnly();

    internal Propriedade() { Nome = null!; }

    public static PropriedadeBuilder Nova => new PropriedadeBuilder();

    public override string ToString() => Nome;
}
