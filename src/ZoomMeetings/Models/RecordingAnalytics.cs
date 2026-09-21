using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /meetings/{meetingId}/recordings/analytics_details.</summary>
public class RecordingAnalyticsDetailsResult
{
    [JsonPropertyName("from")]
    public DateTimeOffset? From { get; set; }

    [JsonPropertyName("to")]
    public DateTimeOffset? To { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("analytics_details")]
    public List<RecordingAnalyticsDetail>? AnalyticsDetails { get; set; }
}

public class RecordingAnalyticsDetail
{
    [JsonPropertyName("date_time")]
    public DateTimeOffset? DateTime { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }
}

/// <summary>Response body for GET /meetings/{meetingId}/recordings/analytics_summary.</summary>
public class RecordingAnalyticsSummaryResult
{
    [JsonPropertyName("from")]
    public DateTimeOffset? From { get; set; }

    [JsonPropertyName("to")]
    public DateTimeOffset? To { get; set; }

    [JsonPropertyName("analytics_summary")]
    public List<RecordingAnalyticsSummaryEntry>? AnalyticsSummary { get; set; }
}

public class RecordingAnalyticsSummaryEntry
{
    [JsonPropertyName("date")]
    public DateTimeOffset? Date { get; set; }

    [JsonPropertyName("views_total_count")]
    public int? ViewsTotalCount { get; set; }

    [JsonPropertyName("downloads_total_count")]
    public int? DownloadsTotalCount { get; set; }
}
