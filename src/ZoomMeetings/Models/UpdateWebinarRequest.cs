using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Request body for ZoomClient.UpdateWebinarAsync (PATCH /webinars/{webinarId}). Only set the fields you want to change.</summary>
public class UpdateWebinarRequest
{
    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("agenda")]
    public string? Agenda { get; set; }

    [JsonPropertyName("settings")]
    public MeetingSettings? Settings { get; set; }
}
