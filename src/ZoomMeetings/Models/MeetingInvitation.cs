using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /meetings/{meetingId}/invitation.</summary>
public class MeetingInvitation
{
    [JsonPropertyName("invitation")]
    public string? Invitation { get; set; }

    [JsonPropertyName("sip_links")]
    public List<string>? SipLinks { get; set; }
}
