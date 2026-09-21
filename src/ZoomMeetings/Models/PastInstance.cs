using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body shared by GET /past_meetings/{meetingId}/instances and GET /past_webinars/{webinarId}/instances.</summary>
public class ListPastInstancesResult
{
    [JsonPropertyName("meetings")]
    public List<PastInstance>? Meetings { get; set; }

    [JsonPropertyName("webinars")]
    public List<PastInstance>? Webinars { get; set; }
}

public class PastInstance
{
    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("occurrence_id")]
    public string? OccurrenceId { get; set; }
}
