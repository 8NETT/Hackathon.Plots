using Application.Messaging;
using Application.Messaging.Events;
using Azure.Messaging.EventHubs;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Messaging;

public sealed class AzureEventHubPublisher : IEventPublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly EventHubProducerClient _producer;
    private readonly ILogger<AzureEventHubPublisher> _logger;

    public AzureEventHubPublisher(EventHubProducerClient producer, ILogger<AzureEventHubPublisher> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    public async Task PublishAsync(Event @event, CancellationToken cancellation)
    {
        // Serializa o evento
        var json = JsonSerializer.Serialize(@event, @event.GetType(), JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);
        var eventData = new EventData(bytes);


        // Metadados úteis para roteamento/observabilidade
        eventData.Properties["type"] = @event.Type;
        eventData.Properties["eventId"] = @event.Id.ToString();
        eventData.ContentType = "application/json";
        
        // Envia o evento
        await _producer.SendAsync([eventData], cancellation);
        _logger.LogInformation("Publicado evento {EventType} com ID {EventId}", @event.Type, @event.Id);
    }
}
