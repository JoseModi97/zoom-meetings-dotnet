using System.Text.Json.Serialization;

namespace ZoomMeetings.Internal;

/// <summary>
/// Zoom's real (but undocumented in the public OpenAPI spec) JSON error shape: { "code": ..., "message": "..." }.
/// </summary>
internal sealed class ZoomErrorBody
{
    [JsonPropertyName("code")]
    public int? Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
