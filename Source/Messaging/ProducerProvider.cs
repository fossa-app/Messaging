namespace Fossa.Messaging;

using Confluent.Kafka;
using Microsoft.Extensions.Options;

/// <summary>
/// Provides an <see cref="IProducer{TKey, TValue}"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProducerProvider"/> class.
/// </remarks>
/// <param name="options">The options.</param>
public class ProducerProvider(IOptions<MessagingOptions> options) : IProducerProvider, IDisposable
{
    private readonly IOptions<MessagingOptions> options = options ?? throw new ArgumentNullException(nameof(options));
    private readonly Lock producerLock = new();
    private bool disposedValue;
    private IProducer<string?, byte[]>? producer;

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public IProducer<string?, byte[]> GetProducer()
    {
        if (this.producer == null)
        {
            lock (this.producerLock)
            {
#pragma warning disable CA1508 // Avoid dead conditional code
                this.producer ??= new ProducerBuilder<string?, byte[]>(this.options.Value.Actor).Build();
#pragma warning restore CA1508 // Avoid dead conditional code
            }
        }

        return this.producer;
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="ProducerProvider"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            this.disposedValue = true;
        }
    }
}
