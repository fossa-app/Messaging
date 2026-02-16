namespace Fossa.Messaging;

using System.Threading.Tasks;
using Confluent.Kafka;
using Google.Protobuf;

/// <summary>
/// Publishes messages to a message broker.
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message to the specified topic.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="entityName">The entity name.</param>
    /// <param name="entityId">The entity identifier.</param>
    /// <param name="cancellationToken">The Cancellation Token.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.</returns>
    public Task<DeliveryResult<string?, byte[]>> PublishAsync(
        IMessage message,
        long companyId,
        string entityName,
        long entityId,
        CancellationToken cancellationToken);
}
