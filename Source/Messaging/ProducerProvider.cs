namespace Fossa.Messaging;

using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using static LanguageExt.Prelude;

/// <summary>
/// Provides an <see cref="IProducer{TKey, TValue}"/>.
/// </summary>
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
                    var messagingActorEntryOptions = this.options.Value.Actor ?? [];
                    var messagingActorOptions = messagingActorEntryOptions
                            .ToDictionary(
                        k => k?.Key ?? throw new InvalidOperationException("One of the Message Actor Entry Options Keys is not provided."),
                        ResolveActorEntryValue);
                    var producerConfig = new ProducerConfig(messagingActorOptions)
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

    private static string ResolveActorEntryValue(MessagingActorEntryOptions? options)
    {
        if (options is null)
        {
            throw new InvalidOperationException("One of the Message Actor Entry Options is null.");
        }

        var providedValues = Seq(
            Tuple(nameof(options.PlainTextValue), Optional(options.PlainTextValue)),
            Tuple(nameof(options.Base64Value), Optional(options.Base64Value)
                .Map(x => Encoding.UTF8.GetString(Convert.FromBase64String(x)))))
            .Choose(x => x.Item2.Map(v => Tuple(x.Item1, v)));

        if (providedValues.Count == 1)
        {
            return providedValues.Single().Item2;
        }
        else if (providedValues.Count == 0)
        {
            throw new InvalidOperationException($"Messaging actor entry '{options.Key}'. One of the value properties must be set.");
        }
        else
        {
            throw new InvalidOperationException($"Messaging actor entry '{options.Key}' has multiple value properties set. Only one of these '{providedValues.Select(x => x.Item1)}' properties should be set.");
        }
    }
}
