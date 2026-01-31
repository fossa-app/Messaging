namespace Fossa.Messaging.Test;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdGen.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using TIKSN.DependencyInjection;
using Xunit;

public class ServiceIdentityProviderTests
{
    private readonly IServiceProvider serviceProvider;

    public ServiceIdentityProviderTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {
                    "Messaging:Actor:bootstrap.servers",
                    "app.f.aivencloud.com:19761"
                },
                {
                    "Messaging:Actor:security.protocol",
                    "Ssl"
                },
                {
                    "Messaging:Actor:sasl.mechanism",
                    "ScramSha256"
                },
                {
                    "Messaging:Actor:ssl.ca.pem",
                    "test-ca"
                },
                {
                    "Messaging:Actor:ssl.certificate.pem",
                    "test-cert"
                },
                {
                    "Messaging:Actor:ssl.key.pem",
                    "test-key"
                },
                {
                    "Messaging:Topic",
                    "test"
                },
            })
            .Build();
        var services = new ServiceCollection();

        _ = services.AddMessaging(configuration, "Fossa", Seq("Messaging", "Test"));
        _ = services.AddFrameworkCore();
        _ = services.AddIdGen(9);
        var fakeTimeProvider = new FakeTimeProvider(
            new DateTimeOffset(2022, 9, 24, 0, 0, 0, TimeSpan.Zero));
        _ = services.AddSingleton<TimeProvider>(fakeTimeProvider);

        ContainerBuilder containerBuilder = new();
        _ = containerBuilder.RegisterModule<CoreModule>();
        containerBuilder.Populate(services);

        this.serviceProvider = new AutofacServiceProvider(containerBuilder.Build());
    }

    [Fact]
    public void GivenServiceIdentity_WhenPropertiesAccessed_ThenReturnsExpectedValues()
    {
        // Arrange
        var serviceIdentityProvider = this.serviceProvider.GetRequiredService<IServiceIdentityProvider>();
        var identity = serviceIdentityProvider.GetIdentity();

        // Act & Assert
        Assert.NotEmpty(identity.ServicePath);
        Assert.Contains("Messaging.Test:", identity.ToString(), StringComparison.OrdinalIgnoreCase);
        Assert.Contains(':', identity.ToString());
    }

    [Fact]
    public void GivenServiceIdentityProvider_WhenGetIdentity_ThenReturnsValidIdentity()
    {
        // Arrange
        var serviceIdentityProvider = this.serviceProvider.GetRequiredService<IServiceIdentityProvider>();

        // Act
        var identity = serviceIdentityProvider.GetIdentity();

        // Assert
        Assert.NotNull(identity);
        Assert.False(string.IsNullOrEmpty(identity.ApplicationName));
        Assert.False(string.IsNullOrEmpty(identity.ServicePath));
        Assert.False(string.IsNullOrEmpty(identity.InstanceId.ToString()));
    }

    [Fact]
    public void GivenServiceIdentityProvider_WhenGetIdentityCalledMultipleTimes_ThenReturnsSameInstance()
    {
        // Arrange
        var serviceIdentityProvider = this.serviceProvider.GetRequiredService<IServiceIdentityProvider>();

        // Act
        var identity1 = serviceIdentityProvider.GetIdentity();
        var identity2 = serviceIdentityProvider.GetIdentity();

        // Assert
        Assert.Same(identity1, identity2);
    }
}
