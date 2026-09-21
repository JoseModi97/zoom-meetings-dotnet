using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for ZoomClient.ListWebinarsAsync (GET /users/{userId}/webinars).</summary>
public class ListWebinarsResult
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("webinars")]
    public List<WebinarSummary>? Webinars { get; set; }
}

/// <summary>A lightweight webinar shape as returned by list endpoints.</summary>
public class WebinarSummary
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
