namespace Application.Messaging.Events;

public abstract record Event
{
    public Guid Id { get; }
    public DateTimeOffset OccurredOn { get; }
    public string Type { get; }

    protected internal Event(string type)
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTimeOffset.UtcNow;
        Type = type;
    }
}
