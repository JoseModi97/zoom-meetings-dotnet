using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ZoomMeetings.Internal;

namespace ZoomMeetings;

/// <summary>
/// Client for the Zoom Meetings REST API. Construct directly for simple/console usage, or use
/// ZoomMeetings.AspNetCore's AddZoomMeetings(...) for DI-friendly registration (auth, retry-on-429,
/// and token caching are wired up automatically either way as long as the supplied HttpClient carries
/// ZoomAuthHandler).
///
/// Typed, ergonomic methods live in the ZoomClient.*.cs partial files (Meetings, Registrants, Polls,
/// Recordings, MeetingSummaries, Reports, Webinars) for the Zoom Meetings API's highest-value
/// operations. <see cref="CallAsync{TResponse}"/> reaches every other endpoint in the API immediately:
/// pass the path exactly as Zoom's docs show it (e.g. "/meetings/{meetingId}/batch_polls").
/// </summary>
public sealed partial class ZoomClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger? _logger;
    private readonly bool _ownsHttpClient;
    private readonly string _baseUrl;

    /// <summary>
    /// Creates a client from a plain <see cref="ZoomConfig"/>, wiring up its own HttpClient with the
    /// auth and retry handlers on top of the default transport (<see cref="HttpClientHandler"/>).
    /// Prefer AddZoomMeetings(...) in ASP.NET Core apps so the HttpClient comes from
    /// IHttpClientFactory instead.
    /// </summary>
    public ZoomClient(ZoomConfig config, ILogger<ZoomClient>? logger = null)
        : this(config, innerHandler: null, logger: logger)
    {
    }

    /// <summary>
    /// Creates a client using a caller-supplied transport <see cref="HttpMessageHandler"/> (e.g. a
    /// fake handler in tests). This takes a handler, not a finished HttpClient: DelegatingHandlers
    /// (auth, retry) can only be layered in front of a raw transport handler at construction time,
    /// not attached to an already-built HttpClient afterwards.
    /// </summary>
    public ZoomClient(ZoomConfig config, HttpMessageHandler? innerHandler, ILogger<ZoomClient>? logger = null)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));

        _logger = logger;
        var transport = innerHandler ?? new HttpClientHandler();

        // The token provider gets its own HttpClient over the SAME transport handler, but outside
        // the auth/retry DelegatingHandler chain below (fetching a token can't depend on already
        // having one). disposeHandler:false because `transport`'s lifetime is owned by _httpClient.
        var tokenProvider = new ZoomTokenProvider(config, new HttpClient(transport, disposeHandler: false), logger);

        var authHandler = new ZoomAuthHandler(tokenProvider, logger) { InnerHandler = transport };
        var retryHandler = new ZoomRetryHandler(maxRetries: 3, logger: logger) { InnerHandler = authHandler };

        _httpClient = new HttpClient(retryHandler, disposeHandler: true);
        _ownsHttpClient = true;
        _baseUrl = config.BaseUrl.TrimEnd('/');
    }

    /// <summary>
    /// Used by ZoomMeetings.AspNetCore, where the HttpClient already has ZoomAuthHandler/
    /// ZoomRetryHandler attached via IHttpClientFactory's AddHttpMessageHandler pipeline, and its
    /// lifetime is owned by IHttpClientFactory rather than this ZoomClient instance.
    /// </summary>
    internal ZoomClient(HttpClient preconfiguredHttpClient, string baseUrl, ILogger? logger)
    {
        _httpClient = preconfiguredHttpClient ?? throw new ArgumentNullException(nameof(preconfiguredHttpClient));
        _logger = logger;
        _ownsHttpClient = false;
        _baseUrl = baseUrl.TrimEnd('/');
    }

    /// <summary>
    /// Disposes the underlying HttpClient (and its handler chain, including the transport handler)
    /// when this ZoomClient constructed its own HttpClient. A no-op when the HttpClient came from
    /// IHttpClientFactory (ZoomMeetings.AspNetCore), since its lifetime isn't ours to manage.
    /// </summary>
    public void Dispose()
    {
        if (_ownsHttpClient)
            _httpClient.Dispose();
    }

    /// <summary>
    /// Calls any Zoom Meetings API endpoint and deserializes a JSON response. This is the escape hatch:
    /// every one of the ~186 operations in Zoom's Meetings OpenAPI spec is reachable this way, typed
    /// wrapper or not. <paramref name="path"/> is relative to the API base (e.g. "/meetings/123/polls").
    /// </summary>
    public async Task<TResponse?> CallAsync<TResponse>(
        HttpMethod method,
        string path,
        object? body = null,
        IDictionary<string, string?>? query = null,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(method, path, body, query, cancellationToken).ConfigureAwait(false);
        return await DeserializeAsync<TResponse>(response, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Calls any Zoom Meetings API endpoint without deserializing a response body (for 202/204
    /// responses, or when the caller doesn't need the body).
    /// </summary>
    public async Task CallAsync(
        HttpMethod method,
        string path,
        object? body = null,
        IDictionary<string, string?>? query = null,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(method, path, body, query, cancellationToken).ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string path,
        object? body,
        IDictionary<string, string?>? query,
        CancellationToken cancellationToken)
    {
        var requestUri = BuildRequestUri(path, query);
        using var request = new HttpRequestMessage(method, requestUri);

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body, JsonDefaults.Options);
            request.Content = new StringContent(json, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        _logger?.LogDebug("Zoom API request: {Method} {Uri}", method, requestUri);

        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            try
            {
                await ThrowZoomApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                response.Dispose();
            }
        }

        return response;
    }

    private string BuildRequestUri(string path, IDictionary<string, string?>? query)
    {
        // Built as a full absolute URL rather than relying on HttpClient.BaseAddress + a relative
        // Uri: per RFC 3986, a relative reference starting with "/" (every path in this library) is
        // an "absolute-path reference" and REPLACES the base's path entirely when combined, silently
        // dropping the "/v2" segment from https://api.zoom.us/v2. Always building the full string
        // ourselves sidesteps that pitfall.
        var normalizedPath = path.Length > 0 && path[0] == '/' ? path : "/" + path;
        var sb = new StringBuilder(_baseUrl).Append(normalizedPath);

        if (query == null || query.Count == 0)
            return sb.ToString();

        sb.Append(normalizedPath.IndexOf('?') >= 0 ? '&' : '?');
        var first = true;
        foreach (var kvp in query)
        {
            if (kvp.Value == null) continue;
            if (!first) sb.Append('&');
            sb.Append(Uri.EscapeDataString(kvp.Key)).Append('=').Append(Uri.EscapeDataString(kvp.Value));
            first = false;
        }

        return sb.ToString();
    }

    private static async Task<TResponse?> DeserializeAsync<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return default;

        var stream = await response.Content.ReadAsStreamAsync(
#if NET
            cancellationToken
#endif
        ).ConfigureAwait(false);

        if (stream.Length == 0)
            return default;

        return await JsonSerializer.DeserializeAsync<TResponse>(stream, JsonDefaults.Options, cancellationToken).ConfigureAwait(false);
    }

    private static async Task ThrowZoomApiExceptionAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var body = await response.Content.ReadAsStringAsync(
#if NET
            cancellationToken
#endif
        ).ConfigureAwait(false);

        ZoomErrorBody? parsed = null;
        try
        {
            parsed = JsonSerializer.Deserialize<ZoomErrorBody>(body, JsonDefaults.Options);
        }
        catch (JsonException)
        {
            // Zoom's error shape isn't part of the public OpenAPI spec; fall back to the raw body.
        }

        throw new ZoomApiException(response.StatusCode, parsed?.Code, parsed?.Message, body);
    }
}
