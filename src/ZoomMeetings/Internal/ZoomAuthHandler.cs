using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace ZoomMeetings.Internal;

/// <summary>
/// DelegatingHandler that attaches "Authorization: Bearer {token}" to every outgoing request,
/// fetching/caching the token via <see cref="ZoomTokenProvider"/>. On a 401, invalidates the cached
/// token and retries exactly once with a freshly fetched one (handles the token being revoked/expired
/// slightly earlier than our local cache believed).
/// </summary>
public sealed class ZoomAuthHandler : DelegatingHandler
{
    private readonly ZoomTokenProvider _tokenProvider;
    private readonly ILogger? _logger;

    internal ZoomAuthHandler(ZoomTokenProvider tokenProvider, ILogger? logger = null)
    {
        _tokenProvider = tokenProvider;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger?.LogDebug("Zoom API returned 401; invalidating cached token and retrying once");
            response.Dispose();
            _tokenProvider.Invalidate();

            var retryToken = await _tokenProvider.GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
            using var retryRequest = await CloneRequestAsync(request).ConfigureAwait(false);
            retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", retryToken);
            return await base.SendAsync(retryRequest, cancellationToken).ConfigureAwait(false);
        }

        return response;
    }

    private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage original)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri)
        {
            Version = original.Version,
        };

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

#if NET
        foreach (var option in original.Options)
            clone.Options.TryAdd(option.Key, option.Value);
#else
        foreach (var property in original.Properties)
            clone.Properties[property.Key] = property.Value;
#endif

        return clone;
    }
}
