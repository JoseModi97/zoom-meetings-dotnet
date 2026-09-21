using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>An AI-generated meeting or webinar summary.</summary>
public class MeetingSummaryDetail
{
    [JsonPropertyName("meeting_host_id")]
    public string? MeetingHostId { get; set; }

    [JsonPropertyName("meeting_host_email")]
    public string? MeetingHostEmail { get; set; }

    [JsonPropertyName("meeting_uuid")]
    public string? MeetingUuid { get; set; }

    [JsonPropertyName("meeting_id")]
    public long MeetingId { get; set; }

    [JsonPropertyName("meeting_topic")]
    public string? MeetingTopic { get; set; }

    [JsonPropertyName("meeting_start_time")]
    public DateTimeOffset? MeetingStartTime { get; set; }

    [JsonPropertyName("meeting_end_time")]
    public DateTimeOffset? MeetingEndTime { get; set; }

    [JsonPropertyName("summary_overview")]
    public string? SummaryOverview { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

/// <summary>Summary of a meeting/webinar summary, as returned by the list endpoints.</summary>
public class MeetingSummaryListItem
{
    [JsonPropertyName("meeting_id")]
    public long MeetingId { get; set; }

    [JsonPropertyName("meeting_uuid")]
    public string? MeetingUuid { get; set; }

    [JsonPropertyName("meeting_topic")]
    public string? MeetingTopic { get; set; }

    [JsonPropertyName("meeting_start_time")]
    public DateTimeOffset? MeetingStartTime { get; set; }

    [JsonPropertyName("meeting_end_time")]
    public DateTimeOffset? MeetingEndTime { get; set; }
}

/// <summary>Response body for listing meeting/webinar summaries.</summary>
public class ListMeetingSummariesResult
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("from")]
    public DateTimeOffset? From { get; set; }

    [JsonPropertyName("to")]
    public DateTimeOffset? To { get; set; }

    [JsonPropertyName("summaries")]
    public List<MeetingSummaryListItem>? Summaries { get; set; }
}
