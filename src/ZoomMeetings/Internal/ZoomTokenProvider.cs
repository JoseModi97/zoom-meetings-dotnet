using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace ZoomMeetings.Internal;

/// <summary>
/// Fetches and caches a Server-to-Server OAuth access token (the "account_credentials" grant) from
/// Zoom's token endpoint, refreshing it shortly before it expires. One instance is shared by a
/// ZoomClient across all of its requests.
/// </summary>
internal sealed class ZoomTokenProvider
{
    private static readonly TimeSpan RefreshSkew = TimeSpan.FromSeconds(60);

    private readonly ZoomConfig _config;
    private readonly HttpClient _httpClient;
    private readonly ILogger? _logger;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private string? _cachedToken;
    private DateTimeOffset _expiresAt = DateTimeOffset.MinValue;

    public ZoomTokenProvider(ZoomConfig config, HttpClient httpClient, ILogger? logger = null)
    {
        _config = config;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        if (_cachedToken != null && DateTimeOffset.UtcNow < _expiresAt - RefreshSkew)
            return _cachedToken;

        await _lock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_cachedToken != null && DateTimeOffset.UtcNow < _expiresAt - RefreshSkew)
                return _cachedToken;

            _config.AssertConfigured();

            var requestUrl = $"{_config.OAuthTokenUrl}?grant_type=account_credentials&account_id={Uri.EscapeDataString(_config.AccountId)}";
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_config.ClientId}:{_config.ClientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

            _logger?.LogDebug("Requesting a new Zoom Server-to-Server OAuth access token");

            using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(
#if NET
                cancellationToken
#endif
            ).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new ZoomApiException(response.StatusCode, null, $"Failed to obtain a Zoom OAuth access token: {body}", body);
            }

            var payload = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(body, JsonDefaults.Options)
                ?? throw new ZoomApiException(response.StatusCode, null, "Zoom OAuth token response could not be parsed.", body);

            if (string.IsNullOrEmpty(payload.AccessToken))
                throw new ZoomApiException(response.StatusCode, null, "Zoom OAuth token response did not contain an access_token.", body);

            // The IsNullOrEmpty check above guarantees this is non-null/non-empty at runtime, but
            // string.IsNullOrEmpty's [NotNullWhen(false)] annotation isn't present on the
            // netstandard2.0 reference assembly, so the compiler can't prove it there - hence "!".
            var accessToken = payload.AccessToken!;
            _cachedToken = accessToken;
            _expiresAt = DateTimeOffset.UtcNow.AddSeconds(payload.ExpiresIn > 0 ? payload.ExpiresIn : 3600);

            return accessToken;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>Forces the next call to GetAccessTokenAsync to fetch a fresh token (e.g. after a 401).</summary>
    public void Invalidate()
    {
        _cachedToken = null;
        _expiresAt = DateTimeOffset.MinValue;
    }

    private sealed class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }
    }
}
