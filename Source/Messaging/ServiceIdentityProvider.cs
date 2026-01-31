namespace Fossa.Messaging;

using IdGen;
using TIKSN.Identity;

/// <summary>
/// Service Identity Provider.
/// </summary>
public class ServiceIdentityProvider : IServiceIdentityProvider
{
    private readonly ServiceIdentity serviceIdentity;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceIdentityProvider"/> class.
    /// </summary>
    /// <param name="idGenerator">The id generator.</param>
    /// <param name="applicationName">The application name.</param>
    /// <param name="componentNames">The component names.</param>
    public ServiceIdentityProvider(IdGenerator idGenerator, string applicationName, Seq<string> componentNames)
    {
        ArgumentNullException.ThrowIfNull(idGenerator);
        ArgumentNullException.ThrowIfNull(applicationName);

        this.serviceIdentity = new ServiceIdentity(applicationName, componentNames, ServiceInstanceId.Create(idGenerator.Id));
    }

    /// <inheritdoc/>
    public ServiceIdentity GetIdentity() => this.serviceIdentity;
}
