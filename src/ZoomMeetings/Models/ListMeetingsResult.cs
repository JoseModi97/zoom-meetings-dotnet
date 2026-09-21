using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for ZoomClient.ListMeetingsAsync (GET /users/{userId}/meetings).</summary>
public class ListMeetingsResult
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("meetings")]
    public List<MeetingSummary>? Meetings { get; set; }
}

/// <summary>A lightweight meeting shape as returned by list endpoints (fewer fields than <see cref="Meeting"/>).</summary>
public class MeetingSummary
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("host_id")]
    public string? HostId { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("join_url")]
    public string? JoinUrl { get; set; }
}
