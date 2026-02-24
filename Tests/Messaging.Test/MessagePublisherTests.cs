namespace Fossa.Messaging.Test;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Fossa.Messaging.Messages.Events;
using IdGen.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using TIKSN.DependencyInjection;
using TIKSN.Identity;
using Xunit;

[Trait("Category", "Integration")]
public class MessagePublisherTests
{
    private readonly IServiceProvider serviceProvider;

    public MessagePublisherTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<MessagePublisherTests>()
            .Build();
        var services = new ServiceCollection();
        _ = services.AddMessaging(configuration, "Fossa", Seq("Messaging", "Test"));
        _ = services.AddFrameworkCore();
        _ = services.AddIdGen(9);

        var fakeTimeProvider = new FakeTimeProvider(
            new DateTimeOffset(2022, 9, 24, 0, 0, 0, TimeSpan.Zero));
        _ = services.AddSingleton<TimeProvider>(fakeTimeProvider);

        var serviceIdentityProvider = Substitute.For<IServiceIdentityProvider>();
        _ = serviceIdentityProvider
            .GetIdentity().Returns(
                new ServiceIdentity(
                    applicationName: "Fossa",
                    componentNames: Seq("Messaging", "Test"),
                    instanceId: ServiceInstanceId.Create(Ulid.NewUlid())));

        _ = services.AddSingleton(serviceIdentityProvider);

        ContainerBuilder containerBuilder = new();
        _ = containerBuilder.RegisterModule<CoreModule>();
        containerBuilder.Populate(services);

        this.serviceProvider = new AutofacServiceProvider(containerBuilder.Build());
    }

    [Fact]
    public async Task GivenPublisherAndMessage_WhenMessageIsPublished_ThenDeliveryShouldSucceedAsync()
    {
        // Arrange
        var messagePublisher = this.serviceProvider.GetRequiredService<IMessagePublisher>();
        const string topic = "test";

        var message = new CompanyDeletedProtoEvent { CompanyId = 123L };

        // Act

        var deliveryResult = await messagePublisher.PublishAsync(message, message.CompanyId, "Company", message.CompanyId, TestContext.Current.CancellationToken).ConfigureAwait(true);

        // Assert

        Assert.NotNull(deliveryResult);
        Assert.NotNull(deliveryResult.Key);
        Assert.NotNull(deliveryResult.Value);
        Assert.Equal(topic, deliveryResult.Topic);
    }
}
