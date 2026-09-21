using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>One row from GET /report/meetings/{meetingId}/participants.</summary>
public class MeetingReportParticipant
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("user_email")]
    public string? UserEmail { get; set; }

    [JsonPropertyName("join_time")]
    public DateTimeOffset? JoinTime { get; set; }

    [JsonPropertyName("leave_time")]
    public DateTimeOffset? LeaveTime { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

/// <summary>Response body for GET /report/meetings/{meetingId}/participants.</summary>
public class ListMeetingReportParticipantsResult
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("page_count")]
    public int PageCount { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("participants")]
    public List<MeetingReportParticipant>? Participants { get; set; }
}
