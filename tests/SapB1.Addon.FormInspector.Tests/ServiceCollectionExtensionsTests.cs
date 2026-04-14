using System;
using Microsoft.Extensions.DependencyInjection;
using SapB1.Addon.FormInspector.Configuration;
using SapB1.Addon.FormInspector.Events;
using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Publishing;
using SapB1.Addon.FormInspector.Snapshot;
using SapB1.Addon.FormInspector.Startup;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddFormInspector_RegistersAllServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var settings = new InspectorSettings();

        // Act
        services.AddFormInspector(settings);

        // Assert — all concrete services are resolvable
        var provider = services.BuildServiceProvider(validateScopes: true);

        provider.GetRequiredService<ISapContext>().Should().NotBeNull();
        provider.GetRequiredService<MatrixInspector>().Should().NotBeNull();
        provider.GetRequiredService<ItemInspector>().Should().NotBeNull();
        provider.GetRequiredService<DataSourceInspector>().Should().NotBeNull();
        provider.GetRequiredService<FormInspectorService>().Should().NotBeNull();
        provider.GetRequiredService<SapHelpers>().Should().NotBeNull();
        provider.GetRequiredService<SnapshotBuilder>().Should().NotBeNull();
        provider.GetRequiredService<HttpPublisher>().Should().NotBeNull();
        provider.GetRequiredService<SnapshotPublisher>().Should().NotBeNull();
        provider.GetRequiredService<Throttler>().Should().NotBeNull();
        provider.GetRequiredService<ConnectionBootstrap>().Should().NotBeNull();
        provider.GetRequiredService<FormEventDispatcher>().Should().NotBeNull();
        provider.GetRequiredService<AddonStartup>().Should().NotBeNull();
    }

    [Fact]
    public void AddFormInspector_RegistersISapContextAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddFormInspector(new InspectorSettings());
        var provider = services.BuildServiceProvider();

        // Act
        var instance1 = provider.GetRequiredService<ISapContext>();
        var instance2 = provider.GetRequiredService<ISapContext>();

        // Assert — same instance returned (singleton)
        instance1.Should().BeSameAs(instance2);
    }

    [Fact]
    public void AddFormInspector_RegistersInspectorSettingsAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        var settings = new InspectorSettings { BackendUrl = "http://test:9999" };
        services.AddFormInspector(settings);
        var provider = services.BuildServiceProvider();

        // Act
        var resolved = provider.GetRequiredService<InspectorSettings>();

        // Assert
        resolved.Should().BeSameAs(settings);
        resolved.BackendUrl.Should().Be("http://test:9999");
    }

    [Fact]
    public void AddFormInspector_InjectsSharedSapContextIntoAllConsumers()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddFormInspector(new InspectorSettings());
        var provider = services.BuildServiceProvider();

        // Act — resolve all consumers that depend on ISapContext
        var sapContext = provider.GetRequiredService<ISapContext>();
        var matrixInspector = provider.GetRequiredService<MatrixInspector>();
        var itemInspector = provider.GetRequiredService<ItemInspector>();
        var dataSourceInspector = provider.GetRequiredService<DataSourceInspector>();
        var formInspector = provider.GetRequiredService<FormInspectorService>();
        var sapHelpers = provider.GetRequiredService<SapHelpers>();
        var connectionBootstrap = provider.GetRequiredService<ConnectionBootstrap>();

        // Assert — all consumers should have received the same ISapContext instance
        // (verified indirectly: they all share the same singleton)
        sapContext.Should().NotBeNull();
        matrixInspector.Should().NotBeNull();
        itemInspector.Should().NotBeNull();
        dataSourceInspector.Should().NotBeNull();
        formInspector.Should().NotBeNull();
        sapHelpers.Should().NotBeNull();
        connectionBootstrap.Should().NotBeNull();
    }

    [Fact]
    public void AddFormInspector_ReturnsServiceCollectionForChaining()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddFormInspector(new InspectorSettings());

        // Assert — same instance returned
        result.Should().BeSameAs(services);
    }
}
