namespace Fossa.Messaging;

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
    /// <returns>Populated Service Collection.</returns>
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        _ = services.AddSingleton<MessageMap>();

        return services;
    }
}
