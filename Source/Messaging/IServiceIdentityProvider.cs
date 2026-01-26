namespace Fossa.Messaging;

using TIKSN.Identity;

/// <summary>
/// Provides service identity information.
/// </summary>
public interface IServiceIdentityProvider
{
    /// <summary>
    /// Gets the service identity.
    /// </summary>
    /// <returns>The service identity.</returns>
    public ServiceIdentity GetIdentity();
}
