using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Request body for ZoomClient.CreateWebinarAsync (POST /users/{userId}/webinars).</summary>
public class CreateWebinarRequest
{
    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("type")]
    public int Type { get; set; } = 5; // 5 = webinar

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
