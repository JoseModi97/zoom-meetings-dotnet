using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ZoomMeetings.AspNetCore;

/// <summary>
/// Verifies Zoom webhook signatures and answers Zoom's required endpoint.url_validation handshake.
/// Zoom signs each webhook request with header "x-zm-signature: v0={hex hmac}" computed over
/// "v0:{x-zm-request-timestamp}:{raw request body}" using your webhook's secret token - this is a
/// distinct credential from the Server-to-Server OAuth client id/secret used for REST calls, found on
/// the same app's "Feature" &gt; "Event Subscriptions" page in the Zoom Marketplace.
/// </summary>
public static class ZoomWebhookHandler
{
    /// <summary>
    /// Verifies a Zoom webhook request's signature. Returns false (never throws on a bad/missing
    /// signature) so callers can respond 401 without leaking why verification failed.
    /// </summary>
    public static bool VerifySignature(string? signatureHeader, string? timestampHeader, string rawBody, string secretToken)
    {
        if (string.IsNullOrEmpty(signatureHeader) || string.IsNullOrEmpty(timestampHeader) || string.IsNullOrEmpty(secretToken))
            return false;

        var expected = ComputeSignature(timestampHeader!, rawBody, secretToken);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected),
            Encoding.UTF8.GetBytes(signatureHeader!));
    }

    /// <summary>Computes the "v0={hex}" signature Zoom expects for a given timestamp/body/secret.</summary>
    public static string ComputeSignature(string timestamp, string rawBody, string secretToken)
    {
        var message = $"v0:{timestamp}:{rawBody}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretToken));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        return "v0=" + Convert.ToHexString(hash).ToLowerInvariant();
    }

    /// <summary>
    /// If <paramref name="payload"/> is Zoom's "endpoint.url_validation" challenge, returns the
    /// { plainToken, encryptedToken } object Zoom requires as the 200 OK response body. Returns null
    /// for every other event, so callers can fall through to their own event handling.
    /// </summary>
    public static object? TryHandleUrlValidation(JsonElement payload, string secretToken)
    {
        if (!payload.TryGetProperty("event", out var eventProp) || eventProp.GetString() != "endpoint.url_validation")
            return null;

        if (!payload.TryGetProperty("payload", out var innerPayload) ||
            !innerPayload.TryGetProperty("plainToken", out var plainTokenProp))
            return null;

        var plainToken = plainTokenProp.GetString() ?? string.Empty;
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretToken));
        var encrypted = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(plainToken))).ToLowerInvariant();

        return new { plainToken, encryptedToken = encrypted };
    }
}
