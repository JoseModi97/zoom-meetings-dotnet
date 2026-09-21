using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZoomMeetings.AspNetCore;
using Xunit;

namespace ZoomMeetings.Tests;

public class ServiceCollectionExtensionsTests
{
    private static void Configure(ZoomConfig config)
    {
        config.AccountId = "acct-1";
        config.ClientId = "client-1";
        config.ClientSecret = "secret-1";
    }

    [Fact]
    public void AddZoomMeetings_NoName_RegistersResolvableSingletonZoomClient()
    {
        var services = new ServiceCollection();
        services.AddZoomMeetings(Configure);

        using var provider = services.BuildServiceProvider();

        var client1 = provider.GetRequiredService<ZoomClient>();
        var client2 = provider.GetRequiredService<ZoomClient>();

        Assert.NotNull(client1);
        Assert.Same(client1, client2); // singleton
    }

    [Fact]
    public void AddZoomMeetings_WithName_RegistersKeyedServiceOnly()
    {
        var services = new ServiceCollection();
        services.AddZoomMeetings(Configure, name: "Sandbox");

        using var provider = services.BuildServiceProvider();

        var keyedClient = provider.GetRequiredKeyedService<ZoomClient>("Sandbox");
        Assert.NotNull(keyedClient);

        // A named/keyed registration should NOT also satisfy the unkeyed resolution.
        var unkeyedClient = provider.GetService<ZoomClient>();
        Assert.Null(unkeyedClient);
    }

    [Fact]
    public void AddZoomMeetings_ProductionAndSandbox_CoexistAsDistinctInstances()
    {
        var services = new ServiceCollection();
        services.AddZoomMeetings(c => { c.AccountId = "prod-acct"; c.ClientId = "prod-client"; c.ClientSecret = "prod-secret"; });
        services.AddZoomMeetings(c => { c.AccountId = "sandbox-acct"; c.ClientId = "sandbox-client"; c.ClientSecret = "sandbox-secret"; }, name: "Sandbox");

        using var provider = services.BuildServiceProvider();

        var production = provider.GetRequiredService<ZoomClient>();
        var sandbox = provider.GetRequiredKeyedService<ZoomClient>("Sandbox");

        Assert.NotNull(production);
        Assert.NotNull(sandbox);
        Assert.NotSame(production, sandbox);
    }

    [Fact]
    public void AddZoomMeetings_FromConfiguration_BindsSectionAndRegistersClient()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Zoom:AccountId"] = "acct-from-config",
                ["Zoom:ClientId"] = "client-from-config",
                ["Zoom:ClientSecret"] = "secret-from-config",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddZoomMeetings(configuration);

        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetRequiredService<ZoomClient>());
    }
}
