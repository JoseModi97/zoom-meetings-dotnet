using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace ZoomMeetings.Internal;

/// <summary>
/// DelegatingHandler that retries a request when Zoom returns 429 Too Many Requests, honoring the
/// Retry-After header when present and falling back to exponential backoff otherwise. Zoom's public
/// OpenAPI spec doesn't document rate-limit response headers, so this deliberately only trusts
/// Retry-After (which Zoom does send in practice) rather than parsing any X-RateLimit-* headers.
/// </summary>
public sealed class ZoomRetryHandler : DelegatingHandler
{
    // TooManyRequests isn't defined on netstandard2.0's System.Net surface.
    private const HttpStatusCode TooManyRequests = (HttpStatusCode)429;

    private readonly int _maxRetries;
    private readonly ILogger? _logger;

    public ZoomRetryHandler(int maxRetries = 3, ILogger? logger = null)
    {
        _maxRetries = maxRetries;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = null;

        for (var attempt = 0; attempt <= _maxRetries; attempt++)
        {
            if (attempt > 0)
            {
                var delay = ComputeDelay(response, attempt);
                _logger?.LogDebug("Zoom API rate limit hit; retrying in {Delay}s (attempt {Attempt}/{Max})", delay.TotalSeconds, attempt, _maxRetries);
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }

            response?.Dispose();

            var clonedRequest = attempt == 0 ? request : await CloneAsync(request).ConfigureAwait(false);
            response = await base.SendAsync(clonedRequest, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode != TooManyRequests || attempt == _maxRetries)
                return response;
        }

        return response!;
    }

    private static TimeSpan ComputeDelay(HttpResponseMessage? response, int attempt)
    {
        if (response?.Headers.RetryAfter?.Delta is TimeSpan delta)
            return delta;

        if (response?.Headers.RetryAfter?.Date is DateTimeOffset date)
        {
            var untilDate = date - DateTimeOffset.UtcNow;
            if (untilDate > TimeSpan.Zero)
                return untilDate;
        }

        return TimeSpan.FromSeconds(Math.Pow(2, attempt));
    }

    private static async Task<HttpRequestMessage> CloneAsync(HttpRequestMessage original)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri);

        if (original.Content != null)
        {
            var bytes = await original.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var content = new ByteArrayContent(bytes);
            foreach (var header in original.Content.Headers)
                content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            clone.Content = content;
        }

        foreach (var header in original.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

        return clone;
    }
}
