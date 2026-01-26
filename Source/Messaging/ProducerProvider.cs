namespace Fossa.Messaging;

using Confluent.Kafka;
using Microsoft.Extensions.Options;

/// <summary>
/// Provides an <see cref="IProducer{TKey, TValue}"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProducerProvider"/> class.
/// </remarks>
/// <param name="serviceIdentityProvider">The service identity provider.</param>
/// <param name="options">The options.</param>
public class ProducerProvider(
    IServiceIdentityProvider serviceIdentityProvider,
    IOptions<MessagingOptions> options) : IProducerProvider, IDisposable
{
    private readonly IOptions<MessagingOptions> options = options ?? throw new ArgumentNullException(nameof(options));
    private readonly Lock producerLock = new();
    private readonly IServiceIdentityProvider serviceIdentityProvider = serviceIdentityProvider ?? throw new ArgumentNullException(nameof(serviceIdentityProvider));

    private bool disposedValue;
    private volatile IProducer<string?, byte[]>? producer;

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public IProducer<string?, byte[]> GetProducer()
    {
        ObjectDisposedException.ThrowIf(this.disposedValue, this);

        if (this.producer == null)
        {
            lock (this.producerLock)
            {
#pragma warning disable CA1508 // Avoid dead conditional code
                if (this.producer == null)
#pragma warning restore CA1508 // Avoid dead conditional code
                {
                    var serviceIdentity = this.serviceIdentityProvider.GetIdentity();
                    var producerConfig = new ProducerConfig(this.options.Value.Actor)
                    {
                        ClientId = serviceIdentity.ToString(),
                    };
                    var producerBuilder = new ProducerBuilder<string?, byte[]>(producerConfig);
                    this.producer = producerBuilder.Build();
                }
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
            if (disposing)
            {
                this.producer?.Dispose();
            }

            this.disposedValue = true;
        }
    }
}
