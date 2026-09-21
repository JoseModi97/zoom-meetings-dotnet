using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZoomMeetings.Internal;

namespace ZoomMeetings.AspNetCore;

/// <summary>
/// DI registration for ZoomClient. Shape mirrors the two-overload (delegate-configure vs.
/// IConfiguration-section-bind) pattern used elsewhere in this ecosystem, with an added optional
/// <c>name</c> parameter: Zoom has no real sandbox API host (confirmed against Zoom's own developer
/// forum), so "environments" are modeled as separate named registrations, each pointing at its own
/// Zoom account/Server-to-Server OAuth app.
/// </summary>
public static class ServiceCollectionExtensions
{
    public const string DefaultConfigurationSection = "Zoom";

    /// <summary>
    /// Registers a ZoomClient configured via a delegate. When <paramref name="name"/> is supplied,
    /// registers it as a keyed service instead (resolve with
    /// <c>[FromKeyedServices(name)] ZoomClient client</c> or
    /// <c>serviceProvider.GetRequiredKeyedService&lt;ZoomClient&gt;(name)</c>) so multiple Zoom
    /// accounts/apps (e.g. "Production" and a dedicated test account registered as "Sandbox") can
    /// coexist in the same service collection.
    /// </summary>
    public static IServiceCollection AddZoomMeetings(
        this IServiceCollection services,
        Action<ZoomConfig>? configure = null,
        string? name = null)
    {
        var config = new ZoomConfig();
        configure?.Invoke(config);
        return services.AddZoomMeetingsCore(config, name);
    }

    /// <summary>
    /// Registers a ZoomClient bound from an IConfiguration section (default "Zoom"). See the
    /// delegate overload's remarks for the <paramref name="name"/>/keyed-service behavior.
    /// </summary>
    public static IServiceCollection AddZoomMeetings(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = DefaultConfigurationSection,
        string? name = null)
    {
        var config = new ZoomConfig();
        configuration.GetSection(sectionName).Bind(config);
        return services.AddZoomMeetingsCore(config, name);
    }

    private static IServiceCollection AddZoomMeetingsCore(this IServiceCollection services, ZoomConfig config, string? name)
    {
        var httpClientName = name == null ? nameof(ZoomClient) : $"{nameof(ZoomClient)}:{name}";

        // The token provider is keyed separately (not just captured via closure into the handler
        // factory below) so it survives IHttpClientFactory's periodic handler-pool rotation
        // (~2 minutes by default) instead of losing its cached token on every rotation.
        services.AddKeyedSingleton(httpClientName, (sp, _) =>
            new ZoomTokenProvider(config, new HttpClient(), sp.GetService<ILogger<ZoomTokenProvider>>()));

        services.AddHttpClient(httpClientName, client => client.BaseAddress = new Uri(config.BaseUrl))
            // Registration order matters: the first-added handler is outermost, so retry (which
            // needs to see 429s and re-invoke the whole inner pipeline, including re-attaching the
            // auth header on each retry) must wrap auth, not the other way around.
            .AddHttpMessageHandler(sp => new ZoomRetryHandler(maxRetries: 3, logger: sp.GetService<ILogger<ZoomRetryHandler>>()))
            .AddHttpMessageHandler(sp => new ZoomAuthHandler(
                sp.GetRequiredKeyedService<ZoomTokenProvider>(httpClientName),
                sp.GetService<ILogger<ZoomAuthHandler>>()));

        if (name == null)
        {
            services.AddSingleton(sp => new ZoomClient(
                sp.GetRequiredService<IHttpClientFactory>().CreateClient(httpClientName),
                config.BaseUrl,
                sp.GetService<ILogger<ZoomClient>>()));
        }
        else
        {
            services.AddKeyedSingleton(name, (sp, _) => new ZoomClient(
                sp.GetRequiredService<IHttpClientFactory>().CreateClient(httpClientName),
                config.BaseUrl,
                sp.GetService<ILogger<ZoomClient>>()));
        }

        return services;
    }
}
