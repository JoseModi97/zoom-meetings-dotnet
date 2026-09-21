using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for POST /meetings/{meetingId}/open_apps.</summary>
public class MeetingAppResult
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("app_id")]
    public string? AppId { get; set; }
}
