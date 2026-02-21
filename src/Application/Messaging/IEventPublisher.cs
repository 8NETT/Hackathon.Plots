using Application.Messaging.Events;

namespace Application.Messaging;

public interface IEventPublisher
{
    Task PublishAsync(Event @event, CancellationToken cancellation);
}
