namespace Application.Messaging.Events;

public sealed record TalhaoCriadoEvent : Event
{
    public required Guid TalhaoId { get; init; }
    public required Guid PropriedadeId { get; init; }
    public required Guid ProprietarioId { get; init; }
    public required string Nome { get; init; }
    public string? Descricao { get; init; }

    public TalhaoCriadoEvent() : base("TalhaoCriado") { }
}
