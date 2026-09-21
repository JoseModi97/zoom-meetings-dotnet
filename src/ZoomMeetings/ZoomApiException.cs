using System.Net;

namespace ZoomMeetings;

/// <summary>
/// Thrown whenever the Zoom API returns a non-2xx response. Carries the HTTP status, Zoom's own
/// { code, message } error body when present, and the raw response body as a fallback (Zoom's error
/// shape isn't part of the public OpenAPI spec, so parsing it is best-effort).
/// </summary>
public class ZoomApiException : Exception
{
    /// <summary>The HTTP status code returned by the Zoom API.</summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>Zoom's own numeric error code, when the response body could be parsed as { code, message }.</summary>
    public int? ZoomCode { get; }

    /// <summary>Zoom's own error message, when the response body could be parsed as { code, message }.</summary>
    public string? ZoomMessage { get; }

    /// <summary>The raw response body, always populated regardless of whether it could be parsed.</summary>
    public string RawBody { get; }

    public ZoomApiException(HttpStatusCode statusCode, int? zoomCode, string? zoomMessage, string rawBody)
        : base(BuildMessage(statusCode, zoomCode, zoomMessage, rawBody))
    {
        StatusCode = statusCode;
        ZoomCode = zoomCode;
        ZoomMessage = zoomMessage;
        RawBody = rawBody;
    }

    private static string BuildMessage(HttpStatusCode statusCode, int? zoomCode, string? zoomMessage, string rawBody)
    {
        if (!string.IsNullOrWhiteSpace(zoomMessage))
            return $"Zoom API request failed with {(int)statusCode} {statusCode}" + (zoomCode is int c ? $" (code {c})" : "") + $": {zoomMessage}";

        var trimmed = rawBody.Length > 500 ? rawBody.Substring(0, 500) + "..." : rawBody;
        return $"Zoom API request failed with {(int)statusCode} {statusCode}: {trimmed}";
    }
}
