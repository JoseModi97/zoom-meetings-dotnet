using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body shared by all of Zoom's jointoken endpoints (local_archiving, local_recording, live_streaming) for both meetings and webinars.</summary>
public class JoinTokenResult
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("expire_in")]
    public long ExpireIn { get; set; }
}

/// <summary>Response body for GET /meetings/{meetingId}/token and GET /webinars/{webinarId}/token.</summary>
public class TokenResult
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }
}
