namespace ZoomMeetings.Internal;

/// <summary>
/// Zoom meeting/webinar UUIDs can contain "/" and, rarely, start with it or contain "//". Per Zoom's
/// own docs, such a UUID must be *double* URL-encoded when used as a path segment (Uri's normal
/// escaping only encodes it once, which the Zoom API then round-trips incorrectly). Plain numeric
/// meeting/webinar IDs are untouched. See the analysis notes in docs/ for why meetingId/webinarId are
/// typed as string throughout this library instead of long, despite the OpenAPI spec typing them
/// inconsistently.
/// </summary>
internal static class ZoomIdEncoding
{
    public static string EncodePathSegment(string id)
    {
        if (string.IsNullOrEmpty(id))
            return id;

        var needsDoubleEncoding = id.IndexOf('/') >= 0;
        var encoded = Uri.EscapeDataString(id);
        return needsDoubleEncoding ? Uri.EscapeDataString(encoded) : encoded;
    }
}
