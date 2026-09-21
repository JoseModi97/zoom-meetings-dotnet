using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /users/{userId}/recordings.</summary>
public class UserRecordingsResult
{
    [JsonPropertyName("from")]
    public DateTimeOffset? From { get; set; }

    [JsonPropertyName("to")]
    public DateTimeOffset? To { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("page_count")]
    public int PageCount { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("meetings")]
    public List<RecordingSet>? Meetings { get; set; }
}
