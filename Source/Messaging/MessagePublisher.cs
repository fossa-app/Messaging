namespace Fossa.Messaging;

using System.Globalization;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Extensions;
using CloudNative.CloudEvents.Kafka;
using Confluent.Kafka;
using Google.Protobuf;
using Microsoft.Extensions.Options;

/// <summary>
/// Publishes messages to a message broker.
/// </summary>
/// <param name="serviceIdentityProvider">The service identity provider.</param>
/// <param name="producerProvider">The producer provider.</param>
/// <param name="messageMap">The Message Map.</param>
/// <param name="options">The options.</param>
/// <param name="timeProvider">The Time Provider.</param>
/// <param name="cloudEventFormatter">The Cloud Event Formatter.</param>
public class MessagePublisher(
    IServiceIdentityProvider serviceIdentityProvider,
    IProducerProvider producerProvider,
    MessageMap messageMap,
    IOptions<MessagingOptions> options,
    TimeProvider timeProvider,
    CloudEventFormatter cloudEventFormatter) : IMessagePublisher
{
    private readonly CloudEventFormatter cloudEventFormatter = cloudEventFormatter ?? throw new ArgumentNullException(nameof(cloudEventFormatter));
    private readonly MessageMap messageMap = messageMap ?? throw new ArgumentNullException(nameof(messageMap));
    private readonly IOptions<MessagingOptions> options = options ?? throw new ArgumentNullException(nameof(options));
    private readonly IProducerProvider producerProvider = producerProvider ?? throw new ArgumentNullException(nameof(producerProvider));
    private readonly IServiceIdentityProvider serviceIdentityProvider = serviceIdentityProvider ?? throw new ArgumentNullException(nameof(serviceIdentityProvider));
    private readonly TimeProvider timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

    /// <inheritdoc/>
    public async Task<DeliveryResult<string?, byte[]>> PublishAsync(
        IMessage message,
        long companyId,
        string entityName,
        long entityId,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var topic = this.options.Value.Topic ?? throw new InvalidOperationException("Topic Configuration is missing");
        var producer = this.producerProvider.GetProducer();
        var cloudEvent = new CloudEvent()
        {
            Data = message,
            Id = Ulid.NewUlid().ToString(),
            DataSchema = null,
            Source = new Uri($"/fossa/agent/{this.serviceIdentityProvider.GetIdentity()}", UriKind.Relative),
            Subject = $"{entityName}/{entityId}",
            Time = this.timeProvider.GetUtcNow(),
            Type = this.messageMap.GetMessageTypeID(message).ToString(CultureInfo.InvariantCulture),
            DataContentType = "application/cloudevents+protobuf",
        }
        .SetPartitionKey($"Company/{companyId}");

        var kafkaMessage = cloudEvent.ToKafkaMessage(ContentMode.Structured, this.cloudEventFormatter);

        return await producer.ProduceAsync(topic, kafkaMessage, cancellationToken).ConfigureAwait(false);
    }
}
