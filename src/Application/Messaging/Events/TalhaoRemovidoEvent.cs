namespace Application.Messaging.Events;

public sealed record TalhaoRemovidoEvent : Event
{
    public Guid TalhaoId { get; }

    public TalhaoRemovidoEvent(Guid talhaoId) : base("TalhaoRemovido")
    {
        TalhaoId = talhaoId;
    }
}
