namespace Fossa.Messaging;

using Confluent.Kafka;

/// <summary>
/// Provides an <see cref="IProducer{TKey, TValue}"/>.
/// </summary>
public interface IProducerProvider
{
    /// <summary>
    /// Gets the producer.
    /// </summary>
    /// <returns>The producer.</returns>
    public IProducer<string?, byte[]> GetProducer();
}
