namespace ZoomMeetings;

/// <summary>
/// Credentials and endpoints for a single Zoom Server-to-Server OAuth app.
/// Zoom does not offer a separate sandbox API host, so "environments" are modeled as
/// separate <see cref="ZoomConfig"/> instances (e.g. one per Zoom account/app) rather than
/// a BaseUrl switch — point a second config at a dedicated test Zoom account instead.
/// </summary>
public class ZoomConfig
{
    /// <summary>The Zoom account ID for a Server-to-Server OAuth app.</summary>
    public string AccountId { get; set; } = Environment.GetEnvironmentVariable("ZOOM_ACCOUNT_ID") ?? string.Empty;

    /// <summary>The Server-to-Server OAuth app's client ID.</summary>
    public string ClientId { get; set; } = Environment.GetEnvironmentVariable("ZOOM_CLIENT_ID") ?? string.Empty;

    /// <summary>The Server-to-Server OAuth app's client secret.</summary>
    public string ClientSecret { get; set; } = Environment.GetEnvironmentVariable("ZOOM_CLIENT_SECRET") ?? string.Empty;

    /// <summary>Base URL for the Zoom REST API. Override only to point at a local mock/proxy during testing.</summary>
    public string BaseUrl { get; set; } = "https://api.zoom.us/v2";

    /// <summary>Token endpoint used for the account_credentials (Server-to-Server) OAuth grant.</summary>
    public string OAuthTokenUrl { get; set; } = "https://zoom.us/oauth/token";

    /// <summary>Optional secret token used to verify inbound Zoom webhook signatures (see ZoomMeetings.AspNetCore).</summary>
    public string? WebhookSecretToken { get; set; }

    internal void AssertConfigured()
    {
        if (string.IsNullOrWhiteSpace(AccountId))
            throw new InvalidOperationException($"{nameof(ZoomConfig)}.{nameof(AccountId)} is required. Set it explicitly or via the ZOOM_ACCOUNT_ID environment variable.");
        if (string.IsNullOrWhiteSpace(ClientId))
            throw new InvalidOperationException($"{nameof(ZoomConfig)}.{nameof(ClientId)} is required. Set it explicitly or via the ZOOM_CLIENT_ID environment variable.");
        if (string.IsNullOrWhiteSpace(ClientSecret))
            throw new InvalidOperationException($"{nameof(ZoomConfig)}.{nameof(ClientSecret)} is required. Set it explicitly or via the ZOOM_CLIENT_SECRET environment variable.");
    }
}
