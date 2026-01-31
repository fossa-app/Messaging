namespace Fossa.Messaging;

using CloudNative.CloudEvents;
using IdGen;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service Collection Extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Messaging.
    /// </summary>
    /// <param name="services">Input Service Collection.</param>
    /// <param name="configuration">Configuration.</param>
    /// <param name="applicationName">The application name.</param>
    /// <param name="componentNames">The component names.</param>
    /// <returns>Populated Service Collection.</returns>
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        string applicationName,
        Seq<string> componentNames)
    {
        _ = services.AddSingleton<MessageMap>();
        _ = services.AddSingleton<IMessagePublisher, MessagePublisher>();
        _ = services.AddSingleton<IProducerProvider, ProducerProvider>();
        _ = services.AddSingleton<CloudEventFormatter, CustomProtobufEventFormatter>();
        _ = services.AddSingleton<IServiceIdentityProvider>(sp =>
            new ServiceIdentityProvider(sp.GetRequiredService<IdGenerator>(), applicationName, componentNames));

        _ = services.Configure<MessagingOptions>(configuration.GetRequiredSection("Messaging"));

        return services;
    }
}
