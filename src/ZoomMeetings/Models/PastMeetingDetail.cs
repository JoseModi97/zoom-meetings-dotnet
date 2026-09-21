using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /past_meetings/{meetingId}.</summary>
public class PastMeetingDetail
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public DateTimeOffset? EndTime { get; set; }

    [JsonPropertyName("host_id")]
    public string? HostId { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("total_minutes")]
    public int? TotalMinutes { get; set; }

    [JsonPropertyName("participants_count")]
    public int? ParticipantsCount { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("user_email")]
    public string? UserEmail { get; set; }

    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }

    [JsonPropertyName("has_meeting_summary")]
    public bool? HasMeetingSummary { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
